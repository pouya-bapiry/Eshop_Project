using Eshop.Application.Services.Interfaces;
using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.ViewComponents
{
    public class UserSidebarDashboardViewComponent : ViewComponent
    {
        #region Fields and ctor
        private readonly IUserService _userService;

        public UserSidebarDashboardViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {

            ViewBag.AvatarImage=await _userService.GetUserImage(User.GetUserId()) ??string.Empty;
            return View("UserSidebarDashboard");
        }

     
    }
    public class UserMobileDashboardViewComponent : ViewComponent
    {
        #region Fields and ctor
        private readonly IUserService _userService;

        public UserMobileDashboardViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {

            ViewBag.AvatarImage = await _userService.GetUserImage(User.GetUserId()) ?? string.Empty;
            return View("UserMobileDashboard");
        }


    }
}