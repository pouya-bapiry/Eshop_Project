

using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Site.Banner;
using Eshop.Domain.Entities.Site;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    #region Slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public HomeSliderViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteImagesService.GetAllActiveSlider();
            return View("HomeSlider", sliders);
        }
    }

    #endregion

    #region Home Banner 1

    public class SiteBannerHome1ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome1ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _siteImagesService.GetBannersByPlacement(BannerPlacement.First);
            return View("SiteBannerHome1", banners);
        }
    }

    #endregion

    #region Home Banner 2

    public class SiteBannerHome2ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome2ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _siteImagesService.GetBannersByPlacement(BannerPlacement.Second);
            return View("SiteBannerHome2", banners);
        }
    }

    #endregion
}
