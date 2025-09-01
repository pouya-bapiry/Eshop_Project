using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;


namespace ServiceHost.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Field and ctor
        private readonly ISiteSettingService _siteSettingService;
        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;

        public HomeController(ISiteSettingService siteSettingService, IContactService contactService, ICaptchaValidator captchaValidator)
        {
            _siteSettingService = siteSettingService;
            _contactService = contactService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region AboutUs
        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var about = await _siteSettingService.GetAboutUs();
            return View(about);
        }

        #endregion

        #region ContactUs
        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            var setting = await _siteSettingService.GetDefaultSiteSetting();
            if (setting == null)
            {
                TempData[ErrorMessage] = "تنظیمات سایت یافت نشد";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.SiteSetting = setting;
            return View();

        }
        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDto contact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();
                await _contactService.CreateContactUs(contact, ip, /*User.GetUserId()*/ null);
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion


        #region NotFound
        [HttpGet("/404-page-not-found")]
        public async Task<IActionResult> PageNotFound()
        {
            return View();
        }
        #endregion

    }
}
