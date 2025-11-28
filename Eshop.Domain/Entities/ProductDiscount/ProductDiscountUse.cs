using Eshop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Entities.ProductDiscount
{
    public class ProductDiscountUse:BaseEntity
    {
        #region Properties

        public long DiscountId { get; set; }

        #endregion

        #region Relations

        public ProductDiscount ProductDiscount { get; set; }

        #endregion
    }

}
