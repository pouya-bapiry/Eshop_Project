using Eshop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    #region Site Header

    public class SiteHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
    }

    #endregion

    #region Site Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public SiteFooterViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSetting = await _siteSettingService.GetDefaultSiteSetting();
            return View("SiteFooter", siteSetting);
        }
    }
    #endregion

    #region Mega Menu

    public class MegaMenuViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public MegaMenuViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _productService.GetAllActiveProductCategories();
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            return View("MegaMenu");
        }
    }
    #endregion

    #region Latest Arrivals

    public class LatestArrivalProductViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestArrivalProductViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestArrival = await _productService.GetLatestArrivalProducts(15);
            return View("LatestArrivalProduct", latestArrival);
        }
    }

    #endregion

    #region Product Discount Amazing

    public class ProductDiscountAmazingViewComponent : ViewComponent
    {
        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountAmazingViewComponent(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var discountAmazing = await _productDiscountService.GetProductDiscountAmazing();
            return View("ProductDiscountAmazing", discountAmazing);
        }
    }
    #endregion
}

