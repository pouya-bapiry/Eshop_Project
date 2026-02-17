using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductComment;
using Eshop.Domain.Entities.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Filter Product

        [HttpGet("products")]
        [HttpGet("products/{Category}")]
        public async Task<IActionResult> FilterProducts(FilterProductDto filter, string title)
        {
            filter.ProductTitle = title;
            filter.TakeEntity = 12;
            filter = await _productService.FilterProducts(filter);

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            //ViewBag.ProductShortView = await _productService.GetProductDetailsBy(1);

            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(filter);
        }
        #endregion

        #region ProductDetail

        [HttpGet("products/{productId}/{title}")]
        public async Task<IActionResult> ProductDetails(long productId,FilterProductCommentDto comment, string title)
        {
            ViewBag.Comment = await _productService.FilterProductComment(comment,productId);
            var product = await _productService.GetProductDetails(productId);

            if (product == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(product);
        }

        #endregion

        #region Product Comment

        #region Create

        [HttpPost("create-product-comment")]
        public async Task<IActionResult> CreateProductComment(CreateProductCommentDto comment, long productId)
        {
            
            if (ModelState.IsValid)
            {
               
                var result = await _productService.CreateProductComment(comment, productId);
                switch (result)
                {
                    case CreateCommentsResult.Success:
                        TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد";
                        return RedirectToAction("ProductDetails", "Product", new { ProductId = productId });

                    case CreateCommentsResult.Error:
                        break;
                    case CreateCommentsResult.NotFound:
                        break;
                    default:
                        break;
                }
            }
            return  RedirectToAction("ProductDetail", "Product" ,new {productId=comment.ProductId});


        }

        #endregion

        #endregion


    }
}
