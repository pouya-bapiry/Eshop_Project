using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductFeatures
{
    public class CreateProductFeatureDto
    {

        public long ProductId { get; set; }

        public long ProductFeatureCategoryId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

        //public List<CreateProductFeatureDto> ProductFeatures { get; set; }
    }

    public enum CreateProductFeatureResult
    {
        Error,
        Success,
        ProductNotFound,
        NotForUserProduct,
        DuplicateFeature
    }
}

