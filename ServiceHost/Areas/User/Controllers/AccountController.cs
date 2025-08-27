using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Account;
using Eshop.Domain.Dtos.Account.User;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;


namespace ServiceHost.Areas.User.Controllers
{
    [Authorize("UserManagement", Roles = Roles.Administrator)]
    public class AccountController : UserBaseController
    {
        #region Fields and ctor
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        public AccountController(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region Edit Profile
        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile(long userId)
        {

            var userProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (userProfile == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(userProfile);

        }

        [HttpPost("edit-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileDto editProfile, IFormFile avatar)
        {

            if (ModelState.IsValid || editProfile.Avatar == null)
            {
                var result = await _userService.EditUserProfile(editProfile, User.GetUserId(), avatar);
                switch (result)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک شده است";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        TempData[InfoMessage] = "لطفا با پشتیبانی سایت تماس حاصل فرمایید تا حساب کاربری تان را فعال نمایند";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = $"{editProfile.FirstName} عزیز، اطلاعات شما با موفقیت ویرایش گردید";
                        return RedirectToAction("Dashboard", "Home");
                    case EditUserProfileResult.NotImage:
                        TempData[ErrorMessage] = "فرمت تصویر آپلود شده شما ساپورت نمیشود ";
                        TempData[InfoMessage] = "لطفا از فرمت های JPG , JPEG , PNG استفاده کنید";
                        return RedirectToAction("EditProfile", "Account");
                }

            }
            return View(editProfile);
        }
        #endregion

        #region ChangePassword


        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {

            if (ModelState.IsValid)
            {
                //var user = await _userService.GetUserById(User.GetUserId());
                //var currentPassword = _passwordHasher.EncodePasswordMd5(passwordDto.CurrentPassword);

              
                    var result = await _userService.ChangeUserPassword(passwordDto, User.GetUserId());

                    switch (result)
                    {
                        case ChangePasswordResult.Success:
                            TempData[SuccessMessage] = "رمز عبور شما تغییر یافت";
                            TempData[InfoMessage] = "لطفا جهت تکمیل فرایند تغییر رمز عبور ، مجددا وارد سایت شوید";
                            await HttpContext.SignOutAsync();
                            return RedirectToAction("Login", "Account", new { area = "" });
                        case ChangePasswordResult.WrongCurrentPassword:
                            TempData[ErrorMessage] = "رمز عبور فعلی شما اشتباه می باشد";
                            break;
                        case ChangePasswordResult.Error:
                            TempData[ErrorMessage] = "در تغییر رمز عبور خطایی روی داد";
                            break;
                        case ChangePasswordResult.NewPasswordSameAsOld:
                            TempData[ErrorMessage] = "رمز عبور جدید باید با رمز عبور فعلی تفاوت داشته باشد ";
                            break;
                    }

            }

            return View(passwordDto);
        }

        #endregion

    }
}