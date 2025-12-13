using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Dtos.ProductFeatures;
using Eshop.Domain.Dtos.ProductFeaturesCategory;
using Eshop.Domain.Dtos.ProductGallery;
using Eshop.Domain.Entities.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Fields and ctor

        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Actions



        #region Product

        #region Filter

        [HttpGet("product-list")]
        public async Task<IActionResult> FilterProduct(FilterProductDto filter)
        {
            var product = await _productService.FilterProductsInAdmin(filter);
            return View(product);
        }

        #endregion

        #region Create

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            var model = new CreateProductDto();
            return View(model);

        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {
            //if (ModelState.IsValid)
            //{
            var result = await _productService.CreateProduct(product, productImage);

            switch (result)
            {
                case CreateProductResult.HasNoImage:
                    TempData[WarningMessage] = "لطفا تصویر محصول را آپلود نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case CreateProductResult.ImageErrorType:
                    TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case CreateProductResult.Error:
                    TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد";
                    break;
                case CreateProductResult.Success:
                    TempData[SuccessMessage] = $"محصول مورد نظر با عنوان {product.Title} با موفقیت ثبت شد";
                    return RedirectToAction("FilterProduct", "Product");
            }
            //}


            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #region Edit

        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();

            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDto edit, long productId, IFormFile productImage)
        {
            var result = await _productService.EditProductInAdmin(edit, productImage);

            switch (result)
            {
                case EditProductResult.NotForUser:
                    TempData[WarningMessage] = "در ویرایش اطلاعات خطایی رخ داده است";
                    break;
                case EditProductResult.NotFound:
                    TempData[ErrorMessage] = "اطلاعات وارد شده یافت نشد";
                    break;
                case EditProductResult.ImageErrorType:
                    TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case EditProductResult.Success:
                    TempData[SuccessMessage] = $"ویرایش محصول {edit.Title} با موفقیت انجام شد";
                    return RedirectToAction("FilterProduct", "Product", new { area = "Administration" });

            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View();
        }
        #endregion

        #region Latest Arrival

        [HttpGet("last-arrival")]

        #endregion

        #endregion

        #region Product Category

        #region Filter

        [HttpGet("product-category-list")]
        public async Task<IActionResult> ProductCategoryList(FilterProductCategoryDto filter)
        {
            var productCategories = await _productService.FilterProductCategory(filter);

            if (productCategories == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(productCategories);

        }
        #endregion

        #region Filter Product Sub Category




        [HttpGet("product-sub-category-list/{parentId}/{categoryName}")]
        public async Task<IActionResult> ProductSubCategoryList(FilterProductCategoryDto filter, long? parentId)
        {

            var productSubCategories = await _productService.FilterProductSubCategory(filter, parentId);

            if (productSubCategories == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { area = "" });
            }

            return View(filter);
        }
        #endregion

        #region Create Product Category


        [HttpGet("create-product-category")]
        public async Task<IActionResult> CreateProductCategory()
        {
            return View();
        }

        [HttpPost("create-product-category"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryDto category, IFormFile categoryImage)
        {
            var result = await _productService.CreateProductCategory(category, categoryImage);

            switch (result)
            {
                case CreateProductCategoryResult.ImageErrorType:
                    TempData[ErrorMessage] = "فرمت تصویر صحیح نمی باشد";
                    break;
                case CreateProductCategoryResult.Error:
                    TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                    break;
                case CreateProductCategoryResult.Success:
                    TempData[SuccessMessage] = "افزودن دسته محصول با موفقیت انجام شد";
                    return RedirectToAction("ProductCategoryList", "Product");
            }

            return View();
        }


        #endregion

        #region Create Product SubCategory

        [HttpGet("create-product-sub-category/{parentId}/{categoryName}")]
        public async Task<IActionResult> CreateProductSubCategory(long? parentId)
        {
            return View();
        }

        [HttpPost("create-product-sub-category/{parentId}/{categoryName}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductSubCategory(CreateProductCategoryDto category, IFormFile categoryImage)
        {
            var result = await _productService.CreateProductCategory(category, categoryImage);

            if (string.IsNullOrWhiteSpace(category.Title))
            {
                TempData[ErrorMessage] = "عنوان یا لینک دسته نمی تواند خالی باشد";
            }

            switch (result)
            {
                case CreateProductCategoryResult.ImageErrorType:
                    TempData[ErrorMessage] = "فرمت تصویر صحیح نمی باشد";
                    break;
                case CreateProductCategoryResult.Error:
                    TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                    break;
                case CreateProductCategoryResult.Success:
                    TempData[SuccessMessage] = "افزودن دسته محصول با موفقیت انجام شد";
                    return category.ParentId == null ? RedirectToAction("ProductCategoryList", "Product") :
                        RedirectToAction("ProductSubCategoryList", "Product", new { parentId = category.ParentId, categoryName = category.Title });
            }

            return View();
        }


        #endregion

        #region Edit Product Category

        [HttpGet("edit-product-category/{id}")]
        public async Task<IActionResult> EditProductCategory(long id)
        {
            var productCategory = await _productService.GetProductCategoryForEdit(id);
            return View(productCategory);
        }

        [HttpPost("edit-product-category/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryDto edit, IFormFile categoryImage)
        {
            if (ModelState.IsValid || edit.Image == null)
            {
                var result = await _productService.EditProductCategory(edit, categoryImage);

                switch (result)
                {
                    case EditProductCategoryResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "ویرایش اطلاعات دسته بندی محصول با موفقیت انجام شد";
                        return edit.ParentId == null ? RedirectToAction("ProductCategoryList", "Product") :
                            RedirectToAction("ProductSubCategoryList", "Product", new { parentId = edit.ParentId, categoryName = edit.Title });
                }
            }

            return View();
        }
        #endregion

        #endregion

        #region Product Color

        #region Product color list


        [HttpGet("product-color-list/{productId}")]
        public async Task<IActionResult> FilterProductColor(long productId)
        {
            ViewBag.ProductId = productId;
            //ProductId = productId;
            var productColor = await _productService.GetAllProductColorInAdminPanel(productId);
            if (productColor == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }



            return View(productColor);
        }

        #endregion

        #region Create Color


        [HttpGet("create-product-color/{productId}")]
        public async Task<IActionResult> CreateProductColor(long productId)
        {
            var model = new CreateProductColorDto();
            return View(model);
        }

        [HttpPost("create-product-color/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductColor(CreateProductColorDto color, long productId)
        {
            
                var result = await _productService.CreateProductColor(color, productId);

                switch (result)
                {
                    case CreateProductColorResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                        break;
                    case CreateProductColorResult.ProductNotFound:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductColorResult.DuplicateColor:
                        TempData[WarningMessage] = "رنگ انتخابی وارد شده تکراری می باشد";
                        break;

                    case CreateProductColorResult.Success:
                        TempData[SuccessMessage] = $"رنگ های انتخابی با موفقیت افزوده شدند.";
                        return RedirectToAction("FilterProductColor", "Product", new { area = "Administration", ProductId = productId });
                
            }


            return View(color);
        }

        #endregion

        #region Edit Product Color

        [HttpGet("edit-product-color/{colorId}/")]
        public async Task<IActionResult> EditProductColor(long colorId)
        {
            var productColor = await _productService.GetProductColorForEdit(colorId);

            if (productColor == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(productColor);
        }

        [HttpPost("edit-product-color/{colorId}/")]
        public async Task<IActionResult> EditProductColor(EditProductColorDto edit, long colorId)
        {
            if (ModelState.IsValid)
            {

                var result = await _productService.EditProductColor(edit, colorId);
                switch (result)
                {
                    case EditProductColorResult.ColorNotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditProductColorResult.DuplicateColor:
                        TempData[WarningMessage] = "رنگ انتخابی وارد شده تکراری می باشد";
                        break;
                    case EditProductColorResult.Success:
                        TempData[SuccessMessage] = "ویرایش اطلاعات رنگ محصول با موفقیت انجام شد";
                        return RedirectToAction("FilterProductColor", "Product", new { area = "Administration", productId = edit.ProductId });
                }
            }
            return View();
        }

        #endregion

        #endregion

        #region Product Features

        #region Get

        [HttpGet("filter-product-features/{productId}")]
        public async Task<IActionResult> FilterProductFeatures(long productId)
        {
            ViewBag.ProductId = productId;

            var productFeature = await _productService.GettAllActiveProductFeatures(productId);

            if (productFeature == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { area = "Administration" });
            }

            return View(productFeature);
        }

        #endregion

        #region Create
        [HttpGet("create-product-feature/{productId}")]
        public async Task<IActionResult> CreateProductFeature(long productId)
        {

            var model = new CreateProductFeatureDto();
            ViewBag.category = await _productService.GetAllFeatureCategories();
            return View(model);
        }

        [HttpPost("create-product-feature/{productId}")]
        public async Task<IActionResult> CreateProductFeature(CreateProductFeatureDto feature, long productId)
        {
            var result = await _productService.CreateProductFeature(feature, productId);

            switch (result)
            {
                case CreateProductFeatureResult.Error:
                    TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                    break;
                case CreateProductFeatureResult.ProductNotFound:
                    TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                    break;
                case CreateProductFeatureResult.DuplicateFeature:
                    TempData[WarningMessage] = "ویژگی انتخابی وارد شده تکراری می باشد";
                    break;

                case CreateProductFeatureResult.Success:
                    TempData[SuccessMessage] = $"ویژگی های انتخابی با موفقیت افزوده شدند.";
                    return RedirectToAction("FilterProductFeatures", "Product", new { area = "Administration", ProductId = productId });
            }

            ViewBag.category = await _productService.GetAllFeatureCategories();

            return View();
        }

        #endregion

        #region Category

        #region Get
        [HttpGet("feature-category-list")]
        public async Task<IActionResult> FilterFeatureCategory(FilterProductFeaturesCategoryDto filter)
        {
            var category = await _productService.GetFilterProductFeaturesCategory(filter);
            return View(category);
        }
        #endregion


        #region Create
        [HttpGet("create-feature-category")]
        public async Task<IActionResult> CreateFeatureCategory()
        {
            var model = new CreateProductFeaturesCategoryDto();
            return View(model);
        }

        [HttpPost("create-feature-category")]
        public async Task<IActionResult> CreateFeatureCategory(CreateProductFeaturesCategoryDto category)
        {
            if (ModelState.IsValid)
            {

                var result = await _productService.CreateProductFeatureCategory(category);
                switch (result)
                {
                    case CreateProductFeaturesCategoryResult.Success:
                        TempData[SuccessMessage] = $"عملیات با موفقیت انجام شد";
                        return RedirectToAction("FilterFeatureCategory", "Product");
                        break;
                    case CreateProductFeaturesCategoryResult.Error:
                        break;
                    case CreateProductFeaturesCategoryResult.Duplicate:
                        break;
                    default:
                        break;
                }

            }
            return View();
        }
        #endregion



        #endregion


        #endregion

        #region Product Gallery

        #region Get
        [HttpGet("product-gallery-list/{productId}")]
        public async Task<IActionResult> FilterProductGallery(long productId)
        {
            ViewBag.ProductId = productId;

            var productGallery = await _productService.FilterProductGalleries(productId);

            if (productGallery == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { area = "Administration" });
            }

            return View(productGallery);
        }
        #endregion

        #region Create
        [HttpGet("create-product-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId)
        {
            var model = new CreateProductGallery();
            return View(model);
        }

        [HttpPost("create-product-gallery/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductGallery(CreateProductGallery gallery, long productId, IFormFile imageName)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductGallery(gallery, productId, imageName);

                switch (result)
                {
                    case CreateProductGalleryResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                        break;
                    case CreateProductGalleryResult.ProductNotFound:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductGalleryResult.Success:
                        TempData[SuccessMessage] = $"گالری تصویر با موفقیت افزوده گردید";
                        return RedirectToAction("FilterProductGallery", "Product",
                            new { area = "Administration", ProductId = productId });
                }
            }


            return View(gallery);
        }


        #endregion

        #region Edit
        [HttpGet("Edit-product-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(long gelleryId)
        {
            var model = new CreateProductGallery();
            return View(model);
        }

        [HttpPost("edit-product-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(EditProductGallery gallery, long galleryId, IFormFile imageName)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProductGallery(  gallery , galleryId, imageName);

                switch (result)
                {
                    case EditProductGalleryResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد";
                        break;
                    case EditProductGalleryResult.ProductNotFound:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case EditProductGalleryResult.Success:
                        TempData[SuccessMessage] = $"گالری تصویر با موفقیت ویرایش گردید";
                        return RedirectToAction("FilterProductGallery", "Product",
                            new { area = "Administration", ProductId = gallery.ProductId });
                }
            }


            return View(gallery);
        }
        #endregion

        #endregion
        #endregion

    }
}
