using Eshop.Domain.Dtos.Product;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface IProductService:IAsyncDisposable
{
    #region Product

    Task<FilterProductDto> FilterProducts(FilterProductDto filter);
    Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage);

    #endregion

}