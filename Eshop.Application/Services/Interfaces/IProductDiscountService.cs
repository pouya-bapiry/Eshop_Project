using Eshop.Domain.Dtos.ProductDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Interfaces
{
    public interface IProductDiscountService:IAsyncDisposable
    {
        Task<FilterDiscountDto> FilterProductDiscount(FilterDiscountDto filter);
        Task<CreateDiscountResult> CreateDiscount(CreateDiscountDto discount, long productId);
        Task<EditDiscountDto> GetDiscountForEdit(long discountId);
        Task<EditDiscountResult> EditDiscount(EditDiscountDto edit);
    }
}
