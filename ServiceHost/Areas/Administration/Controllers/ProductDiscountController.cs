using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.ProductDiscount;
using Eshop.Domain.Entities.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class ProductDiscountController : AdminBaseController
    {
        #region Fields and Ctor
        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountController(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }
        #endregion

        #region Actions

        #region FilterDiscount
        [HttpGet("discounts")]
        [HttpGet("discounts/{productId}")]
        public async Task<IActionResult> FilterDiscounts(FilterDiscountDto filter, long productId)
        {
            var productDiscount = await _productDiscountService.FilterProductDiscount(filter);

            ViewBag.ProductId = productId;

            return View(filter);
        }

        #endregion

        #region Create discont

        [HttpGet("create-discount/{productId}")]
        public async Task<IActionResult> CreateDiscount(long productId)
        {

            return View();
        }
        [HttpPost("create-discount/{productId}")]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto discount, long productId)
        {
            if (ModelState.IsValid)
            {
                var result = await _productDiscountService.CreateDiscount(discount, productId);

                switch (result)
                {
                    case CreateDiscountResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت تخفیف مورد نظر با شکست مواجه شد";
                        break;
                    case CreateDiscountResult.ProductNotFound:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateDiscountResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد";
                        return RedirectToAction("FilterDiscounts", new { area = "Administration", ProductId = productId });
                }
            }
            return View(discount);
        }
        #endregion

        #region Edit Discount
        [HttpGet("edit-discount/{discountId}")]
        public async Task<IActionResult> EditDiscount(long discountId)
        {
       
         var edit=await _productDiscountService.GetDiscountForEdit(discountId);
            //if (edit == null) 
            //{ 
            //    return RedirectToAction("PageNotFound");
            //}
            return View(edit);

        }
        [HttpPost("edit-discount/{discountId}")]
        public async Task<IActionResult> EditDiscount(EditDiscountDto edit,long discountId)
        {
         
           
            var result=await _productDiscountService.EditDiscount(edit,discountId);
                switch (result)
                {
                    case EditDiscountResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد";
                        return RedirectToAction("FilterDiscounts", new { area = "Administration", productId = edit.ProductId });
                      
                    case EditDiscountResult.ProductNotFound:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case EditDiscountResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت تخفیف مورد نظر با شکست مواجه شد";
                        break;
                }

            
            return View();
        }



        #endregion

        #endregion

    }
}
