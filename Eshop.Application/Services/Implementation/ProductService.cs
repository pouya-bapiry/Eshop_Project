using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Dtos.ProductFeatures;
using Eshop.Domain.Dtos.ProductFeaturesCategory;
using Eshop.Domain.Dtos.ProductGallery;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Entities.ProductDiscount;
using Eshop.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Eshop.Application.Services.Implementation;

public class ProductService : IProductService
{
    #region Fileds and ctor
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
    private readonly IGenericRepository<ProductSelectedCategory> _productSelectedRepository;
    private readonly IGenericRepository<ProductColor> _productColorRepository;
    private readonly IGenericRepository<ProductFeatures> _productFeaturesRepository;
    private readonly IGenericRepository<ProductFeaturesCategory> _productFeatureCategoryRepository;
    private readonly IGenericRepository<ProductGallery> _productGalleryRepository;
    private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;


    public ProductService(IGenericRepository<Product> productRepository,
        IGenericRepository<ProductCategory> productCategoryRepository,
        IGenericRepository<ProductSelectedCategory> productSelectedRepository,
        IGenericRepository<ProductColor> productColorRepository,
        IGenericRepository<ProductFeaturesCategory> productFeatureCategoryRepository,
        IGenericRepository<ProductFeatures> productFeaturesRepository,
        IGenericRepository<ProductGallery> productGalleryRepository,
        IGenericRepository<ProductDiscount> productDiscountRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productSelectedRepository = productSelectedRepository;
        _productColorRepository = productColorRepository;
        _productFeatureCategoryRepository = productFeatureCategoryRepository;
        _productFeaturesRepository = productFeaturesRepository;
        _productGalleryRepository = productGalleryRepository;
        _productDiscountRepository = productDiscountRepository;
    }

    #endregion

    #region Product

    #region Filter
    public async Task<FilterProductDto> FilterProducts(FilterProductDto filter)
    {
        var query = _productRepository
                .GetQuery()
                .Include(x => x.ProductColors)
                .Include(x => x.ProductFeatures)
                .Include(x => x.ProductGalleries)
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
            default:
                return new FilterProductDto();
        }

        #endregion

        switch (filter.OrderBy)
        {
            case FilterProductOrderBy.CreateDateDescending:
                query = query.OrderByDescending(x => x.CreateDate);
                break;
            case FilterProductOrderBy.PriceAscending:
                query = query.OrderBy(x => x.Price);
                break;
            case FilterProductOrderBy.PriceDescending:
                query = query.OrderByDescending(x => x.Price);
                break;
            case FilterProductOrderBy.ViewDescending:
                query = query.OrderByDescending(x => x.ViewCount);
                break;
            case FilterProductOrderBy.SellCountDescending:
                query = query.OrderByDescending(x => x.SellCount);
                break;
            case FilterProductOrderBy.SellCountAscending:
                query = query.OrderBy(x => x.SellCount);
                break;
            default:
                new FilterProductDto();
                break;
        }


        var expensiveProduct = await query.OrderByDescending(x => x.Price).FirstOrDefaultAsync();

        filter.FilterMaxPrice = expensiveProduct.Price;

        if (filter.SelectedMaxPrice != 0)
        {
            query = query.Where(x =>
                x.Price >= filter.SelectedMinPrice);
            query = query.Where(x =>
                x.Price <= filter.SelectedMaxPrice);
        }
        else
        {
            filter.SelectedMaxPrice = expensiveProduct.Price;
        }


        if (!string.IsNullOrEmpty(filter.Category))
        {
            query = query.Where(x => x.ProductSelectedCategories.Any(x => x.ProductCategory.UrlName == filter.Category));
        }

        if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
        }

        #region Paging

        var productCount = await query.CountAsync();

        var pager = Pager.Build(filter.PageId, productCount, filter.TakeEntity,
            filter.HowManyShowPageAfterAndBefore);

        var allEntities = await query.Paging(pager).ToListAsync();


        #endregion

        return filter.SetPaging(pager).SetProduct(allEntities);
    }

    public async Task<FilterProductDto> FilterProductsInAdmin(FilterProductDto filter)
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
            case FilterProductOrderBy.SellCountDescending:
                query = query.OrderByDescending(x => x.SellCount);
                break;
            case FilterProductOrderBy.SellCountAscending:
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
            .Include(x => x.ProductDiscounts)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        return latestArrival.Count > take ? latestArrival.Skip(14).Take(1).ToList() : latestArrival;
    }



    #endregion

    #region Product Details
    public async Task<ProductDetailsDto> GetProductDetails(long productId)
    {
        var product = await _productRepository
               .GetQuery()
               .AsQueryable()
               .Include(x => x.ProductColors)
               .Include(x => x.ProductFeatures)
               .Include(x => x.ProductDiscounts)
               .Include(x => x.ProductGalleries)
               .Include(x => x.ProductSelectedCategories)
               .ThenInclude(x => x.ProductCategory)
               .FirstOrDefaultAsync(x => x.Id == productId);

        var productDiscount = await _productDiscountRepository
            .GetQuery()
            .Include(x => x.ProductDiscountUse)
            .OrderByDescending(x => x.CreateDate)
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.ExpireDate >= DateTime.Now);

        var selectedCategoriesIds = product.ProductSelectedCategories.Select(x => x.ProductCategoryId).ToList();

        //var relatedProducts = await _productRepository
        //    .GetQuery()
        //    .Include(x => x.ProductDiscounts)
        //    .Where(x => x.ProductSelectedCategories.Any(c => selectedCategoriesIds.Contains(c.ProductCategoryId)) && x.Id != productId)
        //    .ToListAsync();

        product.ViewCount += 1;
        await _productRepository.SaveChanges();

        var productDetail = new ProductDetailsDto
        {
            ProductId = productId,
            Title = product.Title,
            Code = product.Code,
            Price = product.Price,
            Image = product.Image,
            View = product.ViewCount,
            Description = product.Description,
            ProductColors = product.ProductColors.ToList(),
            ProductFeatures = product.ProductFeatures.ToList(),
            ProductGalleries = product.ProductGalleries.Take(10).ToList(),
            ProductDiscount = productDiscount,
            ProductCategories = product.ProductSelectedCategories.Select(x => x.ProductCategory).ToList(),
            //RelatedProducts = relatedProducts,
            //ProductBrand = product.ProductBrand,
            //ProductComments = product.ProductComments
            //    .Where(x => x.CommentAcceptanceState == CommentAcceptanceState.Accepted && !x.IsDelete)
            //    .OrderByDescending(x => x.Id)
            //    .ToList(),
        };

        return productDetail;

    }


    #endregion

    #endregion

    #region Product Color

    #region get

    public async Task<List<FilterProductColorDto>> GetAllProductColorInAdminPanel(long productId)
    {
        return await _productColorRepository
            .GetQuery()
            .AsQueryable()
            .Include(x => x.Product)
            .Where(x => x.ProductId == productId)
            .Select(x => new FilterProductColorDto
            {
                Id = x.Id,
                ProductId = productId,
                ColorName = x.ColorName,
                ColorCode = x.ColorCode,
                Price = x.Price,
                CreateDate = x.CreateDate.ToStringShamsiDate(),
            }).ToListAsync();
    }

    #endregion

    #region Create

    public async Task<CreateProductColorResult> CreateProductColor(CreateProductColorDto color, long productId)
    {
        var product = await _productRepository.GetEntityById(productId);

        if (product == null)
        {
            return CreateProductColorResult.ProductNotFound;
        }
        foreach (var item in color.ProductColors)
        {
            var isDuplicateColorTitle = await _productColorRepository
                .GetQuery()
                .AnyAsync(x => x.ColorName == item.ColorName);

            if (isDuplicateColorTitle)
            {
                return CreateProductColorResult.DuplicateColor;
            }
        }
        await AddProductColors(productId, color.ProductColors);


        await _productColorRepository.SaveChanges();


        return CreateProductColorResult.Success;
    }

    #endregion

    #region Edit

    public async Task<EditProductColorDto> GetProductColorForEdit(long colorId)
    {
        var productColor = await _productColorRepository
            .GetQuery()
            .AsQueryable()
            .Include(x => x.Product)
            .SingleOrDefaultAsync(x => x.Id == colorId);

        if (productColor == null)
        {
            return null;
        }

        return new EditProductColorDto
        {
            Id = productColor.Id,
            ColorName = productColor.ColorName,
            ColorCode = productColor.ColorCode,
            Price = productColor.Price,
            ProductId = productColor.ProductId
        };
    }

    public async Task<EditProductColorResult> EditProductColor(EditProductColorDto color, long colorId)
    {
        var mainColor = await _productColorRepository
            .GetQuery()
            .AsQueryable()
            .Include(x => x.Product)
            .SingleOrDefaultAsync(x => x.Id == colorId);

        if (mainColor == null)
        {
            return EditProductColorResult.ColorNotFound;
        }



        mainColor.ColorName = color.ColorName;
        mainColor.ColorCode = color.ColorCode;
        mainColor.Price = color.Price;
        mainColor.ProductId = color.ProductId;

        var isDuplicateColorTitle =
                   await _productColorRepository.GetQuery().AnyAsync(x => x.ColorName == mainColor.ColorName);

        if (isDuplicateColorTitle) return EditProductColorResult.DuplicateColor;

        _productColorRepository.EditEntity(mainColor);
        _productColorRepository.SaveChanges();
        return EditProductColorResult.Success;
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

    #region Product Features

    #region Get


    public async Task<List<FilterProductFeatureDto>> GettAllActiveProductFeatures(long productId)
    {
        return await _productFeaturesRepository
                 .GetQuery()
                 .AsQueryable()
                .Include(x => x.ProductFeaturesCategory)
                .Where(x => x.ProductId == productId)
                 .Select(x => new FilterProductFeatureDto
                 {
                     Id = x.Id,
                     ProductId = productId,

                     FeatureTitle = x.FeatureTitle,
                     FeatureValue = x.FeatureValue,
                     CreateDate = x.CreateDate.ToStringShamsiDate(),
                 }).ToListAsync();
    }
    #endregion

    #region Create

    public async Task<CreateProductFeatureResult> CreateProductFeature(CreateProductFeatureDto feature, long productId)
    {
        try
        {
            var product = await _productRepository.GetEntityById(productId);

            if (product == null)
            {
                return CreateProductFeatureResult.ProductNotFound;
            }
            var newFeature = new ProductFeatures
            {
                ProductId = feature.ProductId,
                FeatureTitle = feature.FeatureTitle,
                FeatureValue = feature.FeatureValue,
                //ProductFeaturesCategoryId = feature.ProductFeatureCategoryId,

            };
            await _productFeaturesRepository.AddEntity(newFeature);
            await _productFeaturesRepository.SaveChanges();


            return CreateProductFeatureResult.Success;
        }
        catch (Exception ex)
        {
            //نوشتن خطای مورد نظر 
            Logger.ShowError(ex);
            // نمایش پیغام مناسب به کاربر
            return CreateProductFeatureResult.Error;
        }

    }

    #endregion

    #region Edit

    public Task<EditProductFeatureDto> GetProductFeatureForEdit(long featureId)
    {
        throw new NotImplementedException();
    }

    public Task<EditProductFeatureResult> EditProductFeature(EditProductFeatureDto feature)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Category

    #region Get
    public async Task<List<FilterProductFeaturesCategoryDto>> GetFilterProductFeaturesCategory(FilterProductFeaturesCategoryDto filter)
    {
        return await _productFeatureCategoryRepository.GetQuery().AsQueryable().Where(x => x.IsDelete == false).Select(x => new FilterProductFeaturesCategoryDto
        {
            Id = filter.Id,
            FeatureCategoryTitle = filter.FeatureCategoryTitle,
            CreateDate = DateTime.Now.ToStringShamsiDate()
        }).ToListAsync();



    }
    #endregion

    #region Create
    public async Task<CreateProductFeaturesCategoryResult> CreateProductFeatureCategory(CreateProductFeaturesCategoryDto category)
    {
        var newCategory = new ProductFeaturesCategory
        {

            FeatureCategoryTitle = category.FeatureCategoryTitle,


        };

        await _productFeatureCategoryRepository.AddEntity(newCategory);
        await _productFeatureCategoryRepository.SaveChanges();
        return CreateProductFeaturesCategoryResult.Success;
    }
    #endregion
    #region Edit
    public Task<EditProductFeaturesCategoryDto> GetEditProductFeaturesCategoryForEdit(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<EditProductFeaturesCategoryResult> EditProductFeatureCategory(EditProductFeaturesCategoryDto edit)
    {
        throw new NotImplementedException();
    }
    #endregion


    public async Task<List<ProductFeaturesCategory>> GetAllFeatureCategories()
    {
        return await _productFeatureCategoryRepository
            .GetQuery()
            .AsQueryable()
            .Where(x => x.IsDelete == false)
            .Select(x => new ProductFeaturesCategory
            {
                FeatureCategoryTitle = x.FeatureCategoryTitle,

            }).ToListAsync();
    }



    #endregion


    #endregion

    #region ProductGallery
    #region Get
    public async Task<List<FilterProductGallery>> FilterProductGalleries(long productId)
    {
        return await _productGalleryRepository.GetQuery().AsQueryable().Where(x => x.ProductId == productId).Select(x => new FilterProductGallery
        {
            Id = x.Id,
            ProductId = productId,
            ImageName = x.ImageName,
            DisplayPriority = x.DisplayPriority,
            CreateDate = x.CreateDate.ToStringShamsiDate(),
        }).ToListAsync();
    }
    #endregion

    #region Create
    public async Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGallery gallery, long productId, IFormFile galleryImage)
    {
        var product = await _productRepository.GetEntityById(productId);

        if (product == null)
        {
            return CreateProductGalleryResult.ProductNotFound;
        }

        if (galleryImage == null || !galleryImage.IsImage())
        {
            return CreateProductGalleryResult.ImageIsNull;
        }

        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleryImage.FileName);
        galleryImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100, PathExtension.ProductGalleryThumbServer);

        var newGallery = new ProductGallery
        {
            ProductId = productId,
            ImageName = imageName,
            DisplayPriority = gallery.DisplayPriority
        };

        await _productGalleryRepository.AddEntity(newGallery);
        await _productGalleryRepository.SaveChanges();

        return CreateProductGalleryResult.Success;
    }
    #endregion
    #region Edit
    public async Task<EditProductGallery> GetProductGalleryForEdit(long galleryId)
    {
        var gallery = await _productGalleryRepository
            .GetQuery()
            .Include(x => x.Product)
            .SingleOrDefaultAsync(x => x.Id == galleryId);

        if (gallery == null)
        {
            return null;
        }

        return new EditProductGallery
        {
            ImageName = gallery.ImageName,
            DisplayPriority = gallery.DisplayPriority
        };
    }
    public async Task<EditProductGalleryResult> EditProductGallery(EditProductGallery gallery, long galleryId, IFormFile galleryImage)
    {
        var mainGallery = await _productGalleryRepository
            .GetQuery()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == galleryId);

        if (mainGallery == null)
        {
            return EditProductGalleryResult.ProductNotFound;
        }

        if (galleryImage != null)
        {

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleryImage.FileName);
            galleryImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100,
                PathExtension.ProductGalleryThumbServer, mainGallery.ImageName);

            mainGallery.ImageName = imageName;
        }

        mainGallery.DisplayPriority = gallery.DisplayPriority;

        _productGalleryRepository.EditEntity(mainGallery);
        await _productGalleryRepository.SaveChanges();

        return EditProductGalleryResult.Success;
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

    #region Add or Remove Product Color

    public async Task AddProductColors(long productId, List<CreateProductColorDto> colors)
    {
        var productSelectedColor = new List<ProductColor>();

        foreach (var productColor in colors)
        {
            if (productSelectedColor.All(x => x.ColorName != productColor.ColorName))
            {
                productSelectedColor.Add(new ProductColor
                {
                    ProductId = productId,
                    ColorName = productColor.ColorName,
                    ColorCode = productColor.ColorCode,
                    Price = productColor.Price
                });
            }

        }

        await _productColorRepository.AddRangeEntities(productSelectedColor);
    }

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync()
    {
        if (_productRepository != null)
        {
            await _productRepository.DisposeAsync();
        }
        if (_productCategoryRepository != null)
        {
            await _productCategoryRepository.DisposeAsync();
        }
        if (_productColorRepository != null)
        {
            await _productColorRepository.DisposeAsync();
        }
        if (_productSelectedRepository != null)
        {
            await _productSelectedRepository.DisposeAsync();
        }
        if (_productFeatureCategoryRepository != null)
        {
            await _productFeatureCategoryRepository.DisposeAsync();
        }
    }








    #endregion

}