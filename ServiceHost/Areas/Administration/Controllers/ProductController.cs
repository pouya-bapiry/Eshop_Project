using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
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
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
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


            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #region Edit

        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();

            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDto edit, long productId, IFormFile productImage)
        {
            var result = await _productService.EditProductInAdmin(edit, productImage);

            switch (result)
            {
                case EditProductResult.NotForUser:
                    TempData[WarningMessage] = "در ویرایش اطلاعات خطایی رخ داده است";
                    break;
                case EditProductResult.NotFound:
                    TempData[ErrorMessage] = "اطلاعات وارد شده یافت نشد";
                    break;
                case EditProductResult.ImageErrorType:
                    TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case EditProductResult.Success:
                    TempData[SuccessMessage] = $"ویرایش محصول {edit.Title} با موفقیت انجام شد";
                    return RedirectToAction("FilterProduct", "Product", new { area = "Administration" });

            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View();
        }
        #endregion

        #region Latest Arrival

        [HttpGet("last-arrival")]

        #endregion

        #endregion

        #region Product Category

        #region Filter

        [HttpGet("product-category-list")]
        public async Task<IActionResult> ProductCategoryList(FilterProductCategoryDto filter)
        {
            var productCategories = await _productService.FilterProductCategory(filter);

            if (productCategories == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(productCategories);

        }
        #endregion

        #region Filter Product Sub Category




        [HttpGet("product-sub-category-list/{parentId}/{categoryName}")]
        public async Task<IActionResult> ProductSubCategoryList(FilterProductCategoryDto filter, long? parentId)
        {

            var productSubCategories = await _productService.FilterProductSubCategory(filter, parentId);

            if (productSubCategories == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { area = "" });
            }

            return View(filter);
        }
        #endregion

        #region Create Product Category


        [HttpGet("create-product-category")]
        public async Task<IActionResult> CreateProductCategory()
        {
            return View();
        }

        [HttpPost("create-product-category"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryDto category, IFormFile categoryImage)
        {
            var result = await _productService.CreateProductCategory(category, categoryImage);

            switch (result)
            {
                case CreateProductCategoryResult.ImageErrorType:
                    TempData[ErrorMessage] = "فرمت تصویر صحیح نمی باشد";
                    break;
                case CreateProductCategoryResult.Error:
                    TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                    break;
                case CreateProductCategoryResult.Success:
                    TempData[SuccessMessage] = "افزودن دسته محصول با موفقیت انجام شد";
                    return RedirectToAction("ProductCategoryList", "Product");
            }

            return View();
        }


        #endregion
        #region Create Product SubCategory

        [HttpGet("create-product-sub-category/{parentId}/{categoryName}")]
        public async Task<IActionResult> CreateProductSubCategory(long? parentId)
        {
            return View();
        }

        [HttpPost("create-product-sub-category/{parentId}/{categoryName}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductSubCategory(CreateProductCategoryDto category, IFormFile categoryImage)
        {
            var result = await _productService.CreateProductCategory(category, categoryImage);

            if (string.IsNullOrWhiteSpace(category.Title))
            {
                TempData[ErrorMessage] = "عنوان یا لینک دسته نمی تواند خالی باشد";
            }

            switch (result)
            {
                case CreateProductCategoryResult.ImageErrorType:
                    TempData[ErrorMessage] = "فرمت تصویر صحیح نمی باشد";
                    break;
                case CreateProductCategoryResult.Error:
                    TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                    break;
                case CreateProductCategoryResult.Success:
                    TempData[SuccessMessage] = "افزودن دسته محصول با موفقیت انجام شد";
                    return category.ParentId == null ? RedirectToAction("ProductCategoryList", "Product") :
                        RedirectToAction("ProductSubCategoryList", "Product", new { parentId = category.ParentId, categoryName = category.Title });
            }

            return View();
        }


        #endregion

        #region Edit Product Category

        [HttpGet("edit-product-category/{id}")]
        public async Task<IActionResult> EditProductCategory(long id)
        {
            var productCategory = await _productService.GetProductCategoryForEdit(id);
            return View(productCategory);
        }

        [HttpPost("edit-product-category/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryDto edit, IFormFile categoryImage)
        {
            if (ModelState.IsValid||edit.Image == null)
            {
                var result = await _productService.EditProductCategory(edit, categoryImage);
                
                switch (result)
                {
                    case EditProductCategoryResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "ویرایش اطلاعات دسته بندی محصول با موفقیت انجام شد";
                        return edit.ParentId == null ? RedirectToAction("ProductCategoryList", "Product") :
                            RedirectToAction("ProductSubCategoryList", "Product", new { parentId = edit.ParentId, categoryName = edit.Title });
                }
            }

            return View();
        }
        #endregion
        #endregion
        #endregion
    }
}
