using Eshop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entities.Product
{
    public class Product:BaseEntity
    {

        #region Properties

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "کد محصول")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Code { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Description { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool? IsActive { get; set; }

        [Display(Name = "تصویر محصول")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Image { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int? ViewCount { get; set; }

        [Display(Name = "تعداد فروش")]
        public int? SellCount { get; set; }


        #endregion


        #region Relations

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; } 
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductFeatures> ProductFeatures { get; set; }

        #endregion

    }
}
