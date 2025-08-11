using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact.Ticket;
using Eshop.Domain.Entities.Contact.Ticket;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region fields and ctor

        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public TicketController(IContactService contactService, IUserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }

        #endregion

        #region Actions

        #region Filter Ticket

        [HttpGet("user-tickets")]
        public async Task<IActionResult> TicketList(FilterTicketDto filter)
        {
            filter.UserId = User.GetUserId();
            filter.OrderBy = FilterTicketOrder.CreateDateDescending;

            var result = await _contactService.TicketList(filter);
            return View(result);
        }



        #endregion

        #region Add Ticket

        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }
        [HttpPost("add-ticket")]
        public async Task<IActionResult> AddTicket(AddTicketDto ticket)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.AddUserTicket(ticket, User.GetUserId());
                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "همکاران ما به زودی پاسخ شما را خواهند داد";
                        return RedirectToAction("TicketList", "Ticket", new { area = "User" });
                }
            }

            return View(ticket);

        }




        #endregion
        #region Ticket Detail

        [HttpGet("ticket-detail/{ticketId}")]
        public async Task<IActionResult> TicketDetail(long ticketId)
        {

            var ticket = await _contactService.GetTicketDetail(ticketId, User.GetUserId());
            ViewBag.AvatarImage = await _contactService.GetUserAvatarTicket(ticketId) ?? string.Empty;
            ViewBag.AvatarImageAdmin = await _contactService.GetAdminUserAvatarTicket(ticketId) ?? string.Empty;

            if (ticket == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(ticket);
        }
        #endregion

        #region Answer ticket
        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult > AnswerTicket(AnswerTicketDto answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[ErrorMessage] = "لطفا متن تیکت را وارد کنید";
            }
            if (ModelState.IsValid)
            {
                var result=await _contactService.AnswerTicket(answer, User.GetUserId());
                switch (result)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[WarningMessage] = "درصورت تکرار این مورد، دسترسی شما به صورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("TicketList", "Ticket");
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("TicketList", "Ticket");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }

            }
            return RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = answer.Id });
        }
        #endregion

        #region CloseTicket
        [HttpPost("close-ticket"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseTicket (long ticketId)
        {
            var result = await _contactService.CloseTicket(ticketId);
            switch (result)
            {
                case true:
                    TempData[SuccessMessage] = "تیکت با موفقیت بسته شد";
                    return RedirectToAction("TicketList", "Ticket");
                   
              
                case false:
                    TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                    return RedirectToAction("TicketList", "Ticket");
                     
                default:
                 
            }

           
        }
        #endregion


        #endregion
    }
}
