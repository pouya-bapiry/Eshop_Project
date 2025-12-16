using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductDiscount
{
    public class ProductDiscountAmazingDto
    {
        #region Properties

        public long? ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? DiscountNumber { get; set; }
        public string ProductImage { get; set; }
        public int ProductPrice { get; set; }
        public string CreateDate { get; set; }
        public List<Entities.ProductDiscount.ProductDiscount> ProductDiscounts { get; set; }

        #endregion
    }
}
