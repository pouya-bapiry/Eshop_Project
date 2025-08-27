
using Eshop.Domain.Dtos.Site.Slider;
using Eshop.Domain.Entities.Site;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface ISiteImagesService : IAsyncDisposable
{
    #region Slider

    Task<List<Slider>> GetAllActiveSlider();
    Task<List<Slider>> GetAllSlider();
    Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage);
    Task<EditSliderDto> GetSliderForEdit(long sliderId);
    Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage,string username);
    Task<bool> ActiveSlider(long sliderId, string username);
    Task<bool> DeActiveSlider(long sliderId, string username);

    #endregion

    //#region Banners

    //Task<List<SiteBanner>> GetSiteBannersByLocations(List<BannersLocations> locations);
    //Task<List<SiteBanner>> GetAllBanners();
    //Task<CreateBannerResult> CreateBanner(CreateBannerDto banner, IFormFile bannerImage);
    //Task<EditBannerDto> GetBannerForEdit(long bannerId);
    //Task<EditBannerResult> EditBanner(EditBannerDto edit, IFormFile bannerImage);
    //Task<bool> ActiveBanner(long bannerId);
    //Task<bool> DeActiveBanner(long bannerId);

    //#endregion
}