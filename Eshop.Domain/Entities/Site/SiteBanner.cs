using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Site
{
    public class SiteBanner : BaseEntity
    {
        #region Properties

        public long? UserId { get; set; }
        [Display(Name = "تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ImageName { get; set; }

        [Display(Name = "آدرس بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Url { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "سایز بنر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(25, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ColSize { get; set; }

        public BannerPlacement Placement { get; set; }

        #endregion

        public User User { get; set; }
    }

    public enum BannerPlacement
    {
        First,
        Second,
        Third,
        Forth
    }
}
