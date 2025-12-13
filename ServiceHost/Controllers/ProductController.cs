using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
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
        public async Task<IActionResult> ProductDetails(long productId, string title)
        {
            var product = await _productService.GetProductDetails(productId);

            if (product == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(product);
        }

        #endregion


    }
}
