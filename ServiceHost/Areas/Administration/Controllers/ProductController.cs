using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Fields and ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Actions

        #region Product

        #region Filter

        [HttpGet("product-list")]
        public async Task<IActionResult> FilterProduct(FilterProductDto filter)
        {
            var product = await _productService.FilterProducts(filter);
            return View(product);
        }

        #endregion

        #region Create

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            var model = new CreateProductDto();
            return View(model);

        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {
            //if (ModelState.IsValid)
            //{
                var result = await _productService.CreateProduct(product, productImage);

                switch (result)
                {
                    case CreateProductResult.HasNoImage:
                        TempData[WarningMessage] = "لطفا تصویر محصول را آپلود نمایید";
                        TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                        break;
                    case CreateProductResult.ImageErrorType:
                        TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                        TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                        break;
                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول مورد نظر با عنوان {product.Title} با موفقیت ثبت شد";
                        return RedirectToAction("FilterProduct", "Product");
                }
            //}


            //ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #endregion

        #endregion
    }
}
