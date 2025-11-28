using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductDiscount
{
    public class CreateDiscountDto
    {
        public long ProductId { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(0, 100, ErrorMessage = "درصد تخفیف باید بین اعداد 0 تا 100 باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public int Percentage { get; set; }

        [Display(Name = "تاریخ انقضاء")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ExpireDate { get; set; }

        [Display(Name = "تعداد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public int? DiscountNumber { get; set; }
    }
    public enum CreateDiscountResult
    {
        Success,
        ProductNotFound,
        Error
    }
}
