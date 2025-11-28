using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.ProductDiscount;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Entities.ProductDiscount;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Implementation
{
    public class ProductDiscountService : IProductDiscountService
    {
        #region Fields and ctor

        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUse> _productDiscountUseRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductDiscountService(IGenericRepository<ProductDiscount> productDiscountRepository,
            IGenericRepository<ProductDiscountUse> productDiscountUseRepository,
            IGenericRepository<Product> productRepository)
        {
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUseRepository = productDiscountUseRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Filter
        public async Task<FilterDiscountDto> FilterProductDiscount(FilterDiscountDto filter)
        {
            var query = _productDiscountRepository
                .GetQuery()
                .Include(x => x.Product)
                .AsQueryable();
            #region Filter
            if (filter.ProductId != null && filter.ProductId != null)
            {
                query = query.Where(x => x.ProductId == filter.ProductId.Value);
            }

            if (!string.IsNullOrEmpty(filter.ProductTitle))
            {
                query = query.Where(x => EF.Functions.Like(x.Product.Title, $"%{filter.ProductTitle}%")).OrderByDescending(x => x.CreateDate);
            }
            #endregion

            #region Paging

            var productDiscountCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productDiscountCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();

            #endregion


            return filter.SetPaging(pager).SetProductDiscount(allEntities);
        }
        #endregion

        #region Create
        public async Task<CreateDiscountResult> CreateDiscount(CreateDiscountDto discount, long productId)
        {
            var product = _productRepository.GetEntityById(productId);

            if (product == null)
            {
                return CreateDiscountResult.ProductNotFound;
            }

            var newDiscount = new ProductDiscount
            {
                ProductId = product.Id,
                DiscountNumber = discount.DiscountNumber,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime(),
                Percentage = discount.Percentage

            };
            await _productDiscountRepository.AddEntity(newDiscount);
            await _productDiscountRepository.SaveChanges();
            return CreateDiscountResult.Success;

        }
        #endregion

        #region Edit 
        public async Task<EditDiscountDto> GetDiscountForEdit(long discountId)
        {
            var discount = await _productDiscountRepository
             .GetQuery()

             .AsQueryable()
             .FirstOrDefaultAsync(x => x.Id == discountId);

            if (discount == null)
            {
                return null;
            }

            return new EditDiscountDto
            {
                Id = discount.Id,
                DiscountNumber = discount.DiscountNumber,
                ExpireDate = discount.ExpireDate.ToString(),
                Percentage = discount.Percentage

            };
        }
        public async Task<EditDiscountResult> EditDiscount(EditDiscountDto edit)
        {
            var discount = await _productDiscountRepository
          .GetQuery()
          .AsQueryable()
          .FirstOrDefaultAsync(x => x.Id == edit.Id);

            if (discount == null)
            {
                return EditDiscountResult.ProductNotFound;
            }

            edit.DiscountNumber = discount.DiscountNumber;
            edit.Percentage = discount.Percentage;
            edit.ExpireDate = discount.ExpireDate.ToString();

             _productDiscountRepository.EditEntity(discount);
           await _productDiscountRepository.SaveChanges();
            return EditDiscountResult.Success;
        }




        #endregion




        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_productDiscountRepository != null)
            {
                await _productDiscountRepository.DisposeAsync();
            }
            if (_productDiscountUseRepository != null)
            {
                await _productDiscountUseRepository.DisposeAsync();
            }
            if (_productRepository != null)
            {
                await _productRepository.DisposeAsync();
            }
        }

        #endregion
    }
}
