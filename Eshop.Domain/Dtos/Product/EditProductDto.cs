using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Product
{
    public class EditProductDto : CreateProductDto
    {
        public long Id { get; set; }

        [Display(Name = "پیام تایید / عدم تایید محصول")]
        public string ProductAcceptOrRejectDescription { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ProductImage { get; set; }

    }

    public enum EditProductResult
    {
        NotFound,
        NotForUser,
        Success,
        Error,
        HasNoImage,
        ImageErrorType
    }
}
