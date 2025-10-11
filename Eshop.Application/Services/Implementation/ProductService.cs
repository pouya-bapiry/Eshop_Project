using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementation;

public class ProductService : IProductService
{
    #region Fileds and ctor
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
    private readonly IGenericRepository<ProductSelectedCategory> _productSelectedRepository;

    public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productSelectedRepository = productSelectedRepository;
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

        //add product selected category
        if (product.SelectedCategories != null)
        {
            //create product category
            await AddProductSelectedCategories(newProduct.Id, product.SelectedCategories);
            _productSelectedRepository.SaveChanges();
        }

        await _productRepository.AddEntity(newProduct);
        await _productRepository.SaveChanges();

        return CreateProductResult.Success;

    }




    #endregion

    #region Edit

    public async Task<EditProductDto> GetProductForEdit(long productId)
    {
        var product = await _productRepository
            .GetQuery()
            .AsQueryable()
            .SingleOrDefaultAsync
                (x => x.Id == productId);

        var selectedCategories = await _productSelectedRepository
            .GetQuery()
            .AsQueryable()
            .Where
                (x => x.Id == productId && !x.IsDelete)
            .Select
                (x => x.ProductCategoryId).ToListAsync();

        return new EditProductDto
        {
            Id = productId,
            Title = product.Title,
            Code = product.Code,
            Price = product.Price,
            ProductImage = product.Image,
            IsActive = (bool)product.IsActive,
            Description = product.Description,
            SelectedCategories = selectedCategories,
            ShortDescription = product.ShortDescription,
        };
    }

    public async Task<EditProductResult> EditProductInAdmin(EditProductDto product, IFormFile productImage)
    {
        var mainProduct = await _productRepository
            .GetQuery()
            .AsQueryable()
            .SingleOrDefaultAsync(x => x.Id == product.Id);


        if (mainProduct == null)
        {
            return EditProductResult.NotFound;
        }

        mainProduct.Id = product.Id;
        mainProduct.Title = product.Title;
        mainProduct.Code = product.Code;
        mainProduct.Price = product.Price;
        mainProduct.IsActive = product.IsActive;
        mainProduct.Description = product.Description;
        mainProduct.ShortDescription = product.ShortDescription;


        //Product Image

        if (productImage != null && productImage.IsImage())
        {
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer,
                100, 100, PathExtension.ProductThumbServer, mainProduct.Image);

            mainProduct.Image = imageName;
        }


       
            //Remove All Product Categories
            foreach (var categoryId in product.SelectedCategories)
            {
                await RemoveProductSelectedCategories(product.Id, categoryId);
            }

            // Add Product Categories
            await AddProductSelectedCategories(product.Id, product.SelectedCategories);
            await _productSelectedRepository.SaveChanges();
        

        _productRepository.EditEntity(mainProduct);
        await _productRepository.SaveChanges();

        return EditProductResult.Success;
    }

    #endregion

    #region Maximum view

    public async Task<List<Product>> GetProductWithMaximumView(int take)
    {
        var maxView = await _productRepository
            .GetQuery()
            .AsQueryable()
            .Where(x => !x.IsDelete && x.IsActive.Value)
            .OrderByDescending(x => x.ViewCount)
            .Skip(0)
            .Take(take)
            .Distinct()
            .OrderByDescending(x => x.ViewCount)
            .ToListAsync();

        return maxView.Count > take ? maxView.Skip(14).Take(1).ToList() : maxView;
    }

    #endregion

    #region LatestArrival

    public async Task<List<Product>> GetLatestArrivalProducts(int take)
    {
        var latestArrival = await _productRepository
            .GetQuery()
            .AsQueryable()
            .Where(x => !x.IsDelete && x.IsActive.Value)
            .Take(take)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        return latestArrival.Count > take ? latestArrival.Skip(14).Take(1).ToList() : latestArrival;
    }


    #endregion
    #endregion

    #region Product Category

    #region Filter

    public async Task<FilterProductCategoryDto> FilterProductCategory(FilterProductCategoryDto filter)
    {
        var query = _productCategoryRepository
            .GetQuery()
            .Include(x => x.ProductSelectedCategories)
            .Where(x => x.ParentId == null && !x.IsDelete)
            .AsQueryable();
        #region Filter

        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%")).OrderByDescending(x => x.CreateDate);
        }

        #endregion

        #region Paging

        var productCategoryCount = await query.CountAsync();

        var pager = Pager.Build(filter.PageId, productCategoryCount, filter.TakeEntity,
            filter.HowManyShowPageAfterAndBefore);

        var allEntities = await query.Paging(pager).ToListAsync();


        #endregion

        return filter.SetPaging(pager).SetProduct(allEntities);
    }


    #endregion

    #region filter sub category


    public async Task<FilterProductCategoryDto> FilterProductSubCategory(FilterProductCategoryDto filter, long? parentId)
    {
        var query = _productCategoryRepository
            .GetQuery()
            .Include(x => x.ProductSelectedCategories)
            .Where(x => x.ParentId == parentId && !x.IsDelete).AsQueryable();
        #region Filter

        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%")).OrderByDescending(x => x.CreateDate);
        }

        #endregion

        #region Paging

        var productCategoryCount = await query.CountAsync();

        var pager = Pager.Build(filter.PageId, productCategoryCount, filter.TakeEntity,
            filter.HowManyShowPageAfterAndBefore);

        var allEntities = await query.Paging(pager).ToListAsync();


        #endregion

        return filter.SetPaging(pager).SetProduct(allEntities);
    }


    #endregion

    #region GetAllActiveProductCategories

    public async Task<List<ProductCategory>> GetAllActiveProductCategories()
    {
        return await _productCategoryRepository.GetQuery().AsQueryable().Where(x => x.IsActive && !x.IsDelete)
            .ToListAsync();
    }

    #endregion

    #region GetAllProductCategoriesBy


    public async Task<List<ProductCategory>> GetAllProductCategoriesBy(long? parentId)
    {

        if (parentId == null || parentId == 0)
        {
            return await _productCategoryRepository
                .GetQuery()
                .AsQueryable()
                .Where(x => !x.IsDelete && x.IsActive && x.ParentId == null)
                .ToListAsync();
        }

        return await _productCategoryRepository
            .GetQuery()
            .AsQueryable()
            .Where(x => !x.IsDelete && x.IsActive && x.ParentId == parentId)
            .ToListAsync();
    }
    #endregion


    #region Create

    public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto category, IFormFile image)
    {
        try
        {


            if (string.IsNullOrWhiteSpace(category.Title) && string.IsNullOrWhiteSpace(category.UrlName))
            {
                return CreateProductCategoryResult.Error;
            }

            var newCategory = new ProductCategory()
            {
                Title = category.Title,
                UrlName = category.UrlName.Replace(" ", "-"),
                Icon = category.Icon,
                ParentId = category.ParentId ?? null,
                IsActive = true
            };
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer,
                    100, 100, PathExtension.ProductCategoryThumbServer);

                newCategory.Image = imageName;
            }

            await _productCategoryRepository.AddEntity(newCategory);
            await _productCategoryRepository.SaveChanges();
            return CreateProductCategoryResult.Success;

        }
        catch (Exception e)
        {
            return CreateProductCategoryResult.Error;
        }
    }

    #endregion


    #region Edit

    public async Task<EditProductCategoryDto> GetProductCategoryForEdit(long categoryId)
    {
        var category = await _productCategoryRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == categoryId);

        if (category == null)
        {
            return null;
        }

        return new EditProductCategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Title = category.Title,
            Image = category.Image,
            IsActive = category.IsActive,
            Icon = category.Icon,
            UrlName = category.UrlName,
        };
    }

    public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto category, IFormFile image)
    {
        var mainCategory = await _productCategoryRepository
            .GetQuery()
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        if (mainCategory == null)
        {
            return EditProductCategoryResult.NotFound;
        }

        if (image != null && image.IsImage())
        {

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
            image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer, 100, 100,
                PathExtension.ProductCategoryThumbServer, mainCategory.Image);

            mainCategory.Image = imageName;

        }

        mainCategory.Title = category.Title;
        mainCategory.UrlName = category.UrlName.Replace(" ", "-");
        mainCategory.Icon = category.Icon;
        mainCategory.ParentId = category.ParentId;

        _productCategoryRepository.EditEntity(mainCategory);
        await _productCategoryRepository.SaveChanges();

        return EditProductCategoryResult.Success;
    }


    #endregion



    #endregion

    #region Add or Remove Product Category

    public async Task RemoveProductSelectedCategories(long productId, long productCategoryId)
    {
        var selectedCategories = await _productSelectedRepository
            .GetQuery()
            .Where(x => x.ProductId == productId && x.ProductCategoryId == productCategoryId)
            .ToListAsync();

        if (selectedCategories.Any())
        {
            foreach (var category in selectedCategories)
            {
                 _productSelectedRepository.DeleteEntity(category);
            }
        }

        await _productSelectedRepository.SaveChanges();
    }

    public async Task AddProductSelectedCategories(long productId, List<long> selectedCategory)
    {
        var productSelectedCategories = new List<ProductSelectedCategory>();
        foreach (var categoryId in selectedCategory)
        {
            productSelectedCategories.Add(new ProductSelectedCategory()
            {
                ProductCategoryId = categoryId,
                ProductId = productId
            });
        }

        await _productSelectedRepository.AddRangeEntities(productSelectedCategories);
    }

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync()
    {
        if (_productRepository != null)
        {
            await _productRepository.DisposeAsync();
        }
    }






    #endregion


}