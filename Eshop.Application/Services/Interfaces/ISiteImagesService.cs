using Eshop.Domain.Dtos.Site.Banner;
using Eshop.Domain.Dtos.Site.Slider;
using Eshop.Domain.Entities.Site;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Eshop.Application.Services.Interfaces;

public interface ISiteImagesService : IAsyncDisposable
{
    #region Slider

    Task<List<Slider>> GetAllActiveSlider();
    Task<List<Slider>> GetAllSlider();
    Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage,string username);
    Task<EditSliderDto> GetSliderForEdit(long sliderId);
    Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage,string username);
    Task<bool> ActiveSlider(long sliderId, string username);
    Task<bool> DeActiveSlider(long sliderId, string username);

    #endregion

    #region banners


    Task<List<SiteBanner>> GetBannersByPlacement(BannerPlacement placement);
    Task<List<SiteBanner>> GetAllBanners();
    Task<CreateBannerResult> CreateBanner(CreateBannerDto banner, IFormFile bannerImage, string username);
    Task<EditBannerDto> GetBannerForEdit(long bannerId);
    Task<EditBannerResult> EditBanner(EditBannerDto edit, IFormFile bannerImage, string username);
    Task<bool> ActiveBanner(long bannerId, string username);
    Task<bool> DeActiveBanner(long bannerId, string username);
    

    #endregion
}