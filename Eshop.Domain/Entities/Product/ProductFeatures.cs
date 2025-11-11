using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Product
{
    public class ProductFeatures:BaseEntity
    {
        public long ProductId { get; set; }
        public long ProductFeaturesCategoryId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

        #region Relation


        public ProductFeaturesCategory ProductFeaturesCategory { get; set; }
        
        public Product Product { get; set; }

        #endregion
    }
}
