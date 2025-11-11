using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductGallery
{
    public class CreateProductGallery
    {
        [Display(Name = "الویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Image { get; set; }

    }

    public enum CreateProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}

