using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Site;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class HomeController : AdminBaseController
    {

        #region Fields and Ctor

        private readonly IContactService _contactService;
        private readonly IUserService _userService;
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(IContactService contactService, IUserService userService, ISiteSettingService siteSettingService)
        {
            _contactService = contactService;
            _userService = userService;
            _siteSettingService = siteSettingService;
        }

        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            return View();
        }
        #endregion

        #region Site Setting

        #region Get
        [HttpGet("site-setting")]
        public async Task<IActionResult> SiteSetting()
        {
            var setting = await _siteSettingService.GetDefaultSiteSetting();
            return View(setting);
        }
        #endregion



        #region Edit Site Setting

        [HttpGet("edit-sitesetting/{id}")]
        public async Task<IActionResult> EditSiteSetting(long id)
        {
            var edit = await _siteSettingService.GetSiteSettingForEdit(id);

            return View(edit);

        }
        [HttpPost("edit-sitesetting/{id}")]
        public async Task<IActionResult> EditSiteSetting(EditSiteSettingDto edit)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteSettingService.EditSiteSetting(edit, username);

            switch (result)
            {
                case true:
                    return RedirectToAction("SiteSetting", "Home");
                case false:
                    return null;
                default:

            }
            return View(edit);
        }
        #endregion

        #region Contact Us 

        [HttpGet("contact-us-list")]
        public async Task<IActionResult> ContactUsList(FilterContactUs filter)
        {
            var contactUs = await _contactService.FilterContactUs(filter);
            return View(contactUs);
        }

        #endregion

        #region GetAllAboutUs

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUsList()
        {
            var about = await _contactService.GetAll();
            return View(about);
        }

        #endregion

        #region Create AboutUS

        [HttpGet("create-about-us")]
        public async Task<IActionResult> CreateAboutUs()
        {
            return View();
        }

        [HttpPost("create-about-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAboutUs(CreateAboutUsDto create)
        {
            var result = await _contactService.CreateAboutUs(create);

            switch (result)
            {
                case CreateAboutUsResult.Error:
                    TempData[ErrorMessage] = "در افزودن اطلاعات خطایی رخ داد";
                    break;
                case CreateAboutUsResult.Success:
                    TempData[SuccessMessage] = "عملیات ثبت اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("AboutUsList", "Home");
            }

            return View();
        }
        #endregion

        #region Edit AboutUs

        [HttpGet("edit-aboutUs/{id}")]
        public async Task<IActionResult> EditAboutUs(long id)
        {
            var about = await _contactService.GetAboutUsForEdit(id);
            return View(about);
        }

        [HttpPost("edit-aboutUs"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(EditAboutUsDto edit, string HeaderTitle)
        {
            var user = await _userService.GetUserById(User.GetUserId());

            var username = user.FirstName + " " + user.LastName;

            var result = await _contactService.EditAboutUs(edit, username);

            switch (result)
            {
                case EditAboutUsResult.Error:
                    TempData[ErrorMessage] = "فرمت تصویر نادرست می باشد";
                    break;
                case EditAboutUsResult.NotFound:
                    TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                    break;
                case EditAboutUsResult.Success:
                    TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("AboutUsList", "Home");
            }
            return View();
        }
        #endregion


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion



    }
}
