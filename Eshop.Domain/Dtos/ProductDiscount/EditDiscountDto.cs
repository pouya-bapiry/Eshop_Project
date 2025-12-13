using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductDiscount
{
    public class EditDiscountDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductTitle { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public int Percentage { get; set; }
        public string ExpireDate { get; set; }     

        [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
        public int? DiscountNumber { get; set; }
    }
    public enum EditDiscountResult
    {
        Success,
        ProductNotFound,
        Error
    }
}
