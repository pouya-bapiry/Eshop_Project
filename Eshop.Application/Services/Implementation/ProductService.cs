using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementation;

public class ProductService : IProductService
{
    #region Fileds and ctor

    private readonly IGenericRepository<Product> _productRepository;

    public ProductService(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    #endregion

    #region Product

    #region Filter

    public async Task<FilterProductDto> FilterProducts(FilterProductDto filter)
    {

        var query = _productRepository
            .GetQuery()
            .AsQueryable();

        #region State

        switch (filter.ProductState)
        {
            case FilterProductState.All:
                query = query.Where(x => !x.IsDelete);
                break;
            case FilterProductState.Active:
                query = query.Where(x => x.IsActive.Value);
                break;
            case FilterProductState.NotActive:
                query = query.Where(x => !x.IsActive.Value);
                break;
        }

        switch (filter.OrderBy)
        {
            case FilterProductOrderBy.CreateDateDescending:
                query = query.OrderByDescending(x => x.CreateDate);
                break;
            case FilterProductOrderBy.CreateDateAscending:
                query = query.OrderBy(x => x.CreateDate);
                break;
            case FilterProductOrderBy.PriceDescending:
                query = query.OrderByDescending(x => x.Price);
                break;
            case FilterProductOrderBy.PriceAscending:
                query = query.OrderBy(x => x.Price);
                break;
            case FilterProductOrderBy.ViewDescending:
                query = query.OrderByDescending(x => x.ViewCount);
                break;
            case FilterProductOrderBy.CountDescending:
                query = query.OrderByDescending(x => x.SellCount);
                break;
            case FilterProductOrderBy.CountAscending:
                query = query.OrderBy(x => x.SellCount);
                break;
        }

        #region Filter

        if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
        }

        #endregion

        #region Paging

        var productCount = await query.CountAsync();

        var pager = Pager.Build(filter.PageId, productCount, filter.TakeEntity,
            filter.HowManyShowPageAfterAndBefore);

        var allEntities = await query.Paging(pager).ToListAsync();

        #endregion

        return filter.SetPaging(pager).SetProduct(allEntities);

        #endregion
    }



    #endregion

    #region Create


    public async Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage)
    {
        if (productImage == null)
        {
            return CreateProductResult.HasNoImage;
        }

        if (!productImage.IsImage())
        {
            return CreateProductResult.ImageErrorType;
        }

        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
        productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer,
            100, 100, PathExtension.ProductThumbServer);

        //create Product

        var newProduct = new Product
        {
            Title = product.Title,
            Code = product.Code,
            Price = product.Price,
            Image = imageName,
            IsActive = product.IsActive,
            Description = product.Description,
            ShortDescription = product.ShortDescription,
            ViewCount = 0,
            SellCount = 0

        };


        await _productRepository.AddEntity(newProduct);
        await _productRepository.SaveChanges();

        return CreateProductResult.Success;

    }
    



    #endregion

    #endregion



    #region Dispose

    public async ValueTask DisposeAsync()
    {
        if (_productRepository!=null)
        {
            await _productRepository.DisposeAsync();
        }
    }

  

    #endregion


}