using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Account.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Authorize("UserManagement", Roles = Roles.Administrator)]
    public class AccountController : AdminBaseController
    {
        #region Fields and ctor
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region User

        #region UserList

        [HttpGet("user-list")]
        public async Task<IActionResult> UserList(FilterUserDto filter)
        {
            var user = await _userService.FilterUser(filter);
            user.Roles = await _userService.GetRoles();
            ViewBag.roles = user.Roles;
            return View(filter);
        }
        #endregion

        #region EditUser

        [HttpGet("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(long userId)
        {
            var user = await _userService.GetUserForEdit(userId);
            if (user == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            ViewBag.roles = await _userService.GetRoles();


            return View(user);
        }

        [HttpPost("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(EditUserDto edit)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                var username = user.FirstName + " " + user.LastName;
                var result = await _userService.EditUser(edit, username);

                switch (result)
                {
                    case EditUserResult.UserNotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case EditUserResult.Success:
                        TempData[SuccessMessage] = "ویرایش کاربر با موفقیت انجام شد";
                        return RedirectToAction("UserList", "Account");
                }
            }

            ViewBag.roles = await _userService.GetRoles();
            return View();
        }

        #endregion


        #endregion


        #region Role


        #region Role list
        [HttpGet("role-list")]
        public async Task<IActionResult> RoleList(FilterRoleDto filter)
        {
            var role = await _userService.FilterRole(filter);
            return View(filter);
        }

        #endregion

        #region Add Role

        [HttpGet("create-role")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("create-role"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDto role)
        {
            var result = await _userService.CreateRole(role);

            switch (result)
            {
                case CreateRoleResult.Error:
                    TempData[ErrorMessage] = "عملیات مورد نظر با خطا مواجه شد";
                    break;
                case CreateRoleResult.Success:
                    TempData[SuccessMessage] = "افزودن نقش با موفقیت انجام شد";
                    return RedirectToAction("RoleList", "Account");
            }

            return View();

        }

        #endregion

        #region Edit Role

        [HttpGet("edit-role/{roleId}")]
        public async Task<IActionResult> EditRole(long roleId)
        {
            var role = await _userService.GetRoleForEdit(roleId);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost("edit-role/{roleId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleDto edit)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _userService.EditRole(edit, username);

            switch (result)
            {
                case EditRoleResult.Error:
                    TempData[ErrorMessage] = "در ویرایش اطلاعات خطایی رخ داد";
                    break;
                case EditRoleResult.Success:
                    TempData[SuccessMessage] = "ویرایش نقش با موفقیت انجام شد";
                    return RedirectToAction("RoleList", "Account"); ;
            }

            return View();
        }


        #endregion

        #region SignOut

        [HttpGet("log-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #endregion

    }
}
