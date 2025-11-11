using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductGallery
{
    public class FilterProductGallery
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        [Display(Name = "اولویت نمایش")]
        public int? DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        public string CreateDate { get; set; }
    }
}
