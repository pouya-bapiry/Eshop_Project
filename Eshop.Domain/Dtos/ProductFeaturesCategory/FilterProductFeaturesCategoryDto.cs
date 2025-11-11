using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductFeaturesCategory
{
    public class FilterProductFeaturesCategoryDto
    {
        public long Id { get; set; }

        public string FeatureCategoryTitle { get; set; }


        public string CreateDate { get; set; }
       // public List<Entities.Product.ProductFeatureCategory> FeatureCategories { get; set; }
    }
}
