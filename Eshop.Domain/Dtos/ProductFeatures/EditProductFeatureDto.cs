using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductFeatures
{
    public class EditProductFeatureDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

       

      
    }

    public enum EditProductFeatureResult
        {
        Error,
        Success,
        ProductNotFound,
        NotForUserProduct,
        DuplicateFeature
    }  
   
}
