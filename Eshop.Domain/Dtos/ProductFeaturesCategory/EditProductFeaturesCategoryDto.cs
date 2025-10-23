using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductFeaturesCategory
{
    public class EditProductFeaturesCategoryDto
    {
        public long Id { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureCategoryTitle { get; set; }
    }
    public enum EditProductFeaturesCategoryResult
    {
        Success,
        Error,
        Duplicate
    }
}
