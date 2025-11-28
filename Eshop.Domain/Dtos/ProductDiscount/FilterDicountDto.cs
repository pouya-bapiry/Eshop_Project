using Eshop.Domain.Dtos.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.ProductDiscount
{
    public class FilterDiscountDto : BasePaging
    {
        #region Properties

        public long? ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? DiscountNumber { get; set; }
        public string CreateDate { get; set; }
        public List<Entities.ProductDiscount.ProductDiscount> ProductDiscounts { get; set; }

        #endregion

        #region Methods

        public FilterDiscountDto SetProductDiscount(List<Entities.ProductDiscount.ProductDiscount> productDiscounts)
        {
            this.ProductDiscounts = productDiscounts;
            return this;
        }

        public FilterDiscountDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

   

}
