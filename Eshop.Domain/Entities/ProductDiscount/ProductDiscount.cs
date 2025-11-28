using Eshop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Entities.ProductDiscount
{
    public class ProductDiscount:BaseEntity
    {

        #region Properties

        public long ProductId { get; set; }

        [Range(0, 100)]
        [Display(Name = "درصد تخفیف")]
        public int Percentage { get; set; }

        public DateTime ExpireDate { get; set; }
        public int? DiscountNumber { get; set; }


        #endregion

        #region Relations

        public Product.Product Product { get; set; }
        public ICollection<ProductDiscountUse> ProductDiscountUse { get; set; }

        #endregion
    }
}
