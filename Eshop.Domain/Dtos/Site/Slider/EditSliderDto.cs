using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Site.Slider
{
    public class EditSliderDto
    {

        #region Properties
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

       
        [Display(Name = "نام تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ImageName { get; set; }

       
        [Display(Name = "نام تصویر موبایل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? MobileImageName { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }


        #endregion
    }

    public enum EditSliderResult
    {
        Success,
        Error,
        NotFound,
    }
}
