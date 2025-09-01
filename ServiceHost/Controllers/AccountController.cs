using Eshop.Application.Services.Implementation;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account;
using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Repository;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;

namespace ServiceHost.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Fields And Ctor

        private readonly IUserService _userService;
        private readonly ISmsService _smsService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IGenericRepository<User> _userRepository;
        public static string ReturnUrl { get; set; }

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator, IGenericRepository<User> userRepository, ISmsService smsService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _userRepository = userRepository;
            _smsService = smsService;
        }


        #endregion

        #region Actions

        #region Register User

        [HttpGet("register")]
        public async Task<IActionResult> RegisterUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDto register)
        {
            //if (ModelState.IsValid)
            //{
            var result = await _userService.RegisterUser(register);
            var user = await _userService.GetUserByMobile(register.Mobile);

            if (user.IsMobileActive == true)
            {
                TempData[InfoMessage] = "حساب کاربری شما قبلا فعال شده است";
                return RedirectToAction("Login", "Account");
            }
            else
            {

                switch (result)
                {
                    case RegisterUserResult.PasswordError:
                        TempData[InfoMessage] = $"رمز عبور وارد شده با رمز عبوری که قبلا ثبت کرده اید تطابقت ندارد";
                        TempData[WarningMessage] = $"لطفا رمز عبور را به درستی وارد کنید و همچنین حساب کاربری خود را فعال سازی کنید در صورت فراموشی رمز عبور با پشتیبانی تماس بگیرید";
                        return RedirectToAction("RegisterUser", "Account");
                        break;
                    case RegisterUserResult.MobileNotActive:
                        TempData[WarningMessage] = $"شماره همراه {register.Mobile} .فعال نشده است. لطفا شماره همراه خود را فعال کنید";
                        TempData[InfoMessage] = $"کد تایید، جهت فعالسازی حساب کاربری به شماره همراه {register.Mobile} ارسال گردید.";
                        return RedirectToAction("ActivateMobile", "Account", new { mobile = register.Mobile });
                    case RegisterUserResult.MobileExists:
                        TempData[WarningMessage] = $"شماره همراه : {register.Mobile} تکراری می باشد.";
                        ModelState.AddModelError("Mobile", "شماره همراه تکراری می باشد.");
                        break;
                        return RedirectToAction("Index", "Home");
                    case RegisterUserResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case RegisterUserResult.Success:
                        TempData[InfoMessage] = $"کد تایید، جهت فعالسازی حساب کاربری به شماره همراه {register.Mobile} ارسال گردید.";
                        //return RedirectToAction("ActivateMobile", "Account");
                        return RedirectToAction("ActivateMobile", "Account", new { mobile = register.Mobile });
                        break;

                }
            }
            //}
            return View(register);
        }
        #endregion

        #region ActivateMobile


        [HttpGet("activate-mobile/{mobile}")]
        public async Task<IActionResult> ActivateMobile(string mobile)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var activateMobile = new ActiveMobileDto { Mobile = mobile };
            return View(activateMobile);
        }

        [HttpPost("activate-mobile/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActiveMobileDto activate)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activate.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد. چند ثانیه بعد دوباره تلاش کنید";
                TempData[InfoMessage] = "لطفا از اتصال اینترنت خود مطمئن شوید";
                return View(activate);
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByMobile(activate.Mobile);

                if (!user.IsMobileActive)
                {
                    var result = await _userService.ActivateMobile(activate);

                    if (result)
                    {
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        TempData[InfoMessage] = "جهت ورود به حساب خود شماره موبایل و رمز عبور خود را وارد نمایید";
                        return RedirectToAction("Login", "Account");
                    }
                }
                else if (user.IsMobileActive)
                {
                    TempData[InfoMessage] = "حساب کاربری شما قبلا فعال شده است";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData[ErrorMessage] = "کد فعالسازی اشتباه است";
                }

                TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
            }
            return View(activate);
        }


        #endregion

        #region Login

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ReturnUrl = returnUrl;
            return View();

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto login)
        {
            //if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            //{
            //    TempData[ErrorMessage] = "کد کپچای شما تایید نشد. چند ثانیه بعد دوباره تلاش کنید";
            //    TempData[InfoMessage] = "لطفا از اتصال اینترنت خود مطمئن شوید";
            //    return View(login);
            //}
            if (ModelState.IsValid)
            {
                var result = await _userService.UserLogin(login);

                switch (result)
                {
                    case UserLoginResult.UserNotFound:
                        TempData[WarningMessage] = "کاربری با این مشخصات یافت نشد.";
                        ModelState.AddModelError("Mobile", "کاربری با این مشخصات یافت نشد.");
                        break;
                    case UserLoginResult.MobileNotActivated:
                        TempData[WarningMessage] = "شماره همراه شما فعال نشده است.";
                        ModelState.AddModelError("Mobile", "شماره همراه شما فعال نشده است.");
                        break;
                    case UserLoginResult.Error:
                        TempData[ErrorMessage] = "در ورود به حساب کاربری خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case UserLoginResult.IsBlocked:
                        TempData[InfoMessage] = "لطفا با پشتیبانی سایت تماس حاصل فرمایید";
                        TempData[WarningMessage] = "کاربری مورد نظر بلاک شده است. ";
                        ModelState.AddModelError("Mobile", "کاربری با این مشخصات یافت نشد.");
                        break;
                    case UserLoginResult.Success:
                        var user = await _userService.GetUserByMobile(login.Mobile);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.MobilePhone, user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            //new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name,user.FirstName+" "+user.LastName),
                            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var properties = new AuthenticationProperties()
                        {
                            IsPersistent = login.RememberMe,
                            RedirectUri = HttpContext.Request.Query["RedirectUri"]

                        };
                        await HttpContext.SignInAsync(principal, properties);
                        TempData[SuccessMessage] = "شما با موفقیت وارد سایت شدید";

                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return Redirect("/");

                }

            }

            return View(login);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
        #region RecoverPassword


        [HttpGet("recover-password")]
        public async Task<IActionResult> RecoverPassword()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            //var forgetPassword = new ForgotPasswordDto { Mobile = mobile };
            return View();
        }

        [HttpPost("recover-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverPassword(ForgotPasswordDto forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد. چند ثانیه بعد دوباره تلاش کنید";
                TempData[InfoMessage] = "لطفا از اتصال اینترنت خود مطمئن شوید";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);

                switch (result)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        ModelState.AddModelError("Mobile", "کاربر مورد نظر یافت نشد ");
                        break;
                    case ForgotPasswordResult.Error:
                        TempData[ErrorMessage] = "در بازیابی رمز عبور خطایی رخ داد. لطفا مجددا تلاش کنید";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = " رمز عبور جدید برای شما ارسال شد";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری، رمز عبور خود را تغییر دهید";
                        return RedirectToAction("Login", "Account");
                }

            }
            return View(forgot);
        }
        #endregion

        #endregion

    }
}