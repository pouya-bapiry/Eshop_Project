using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;
using Eshop.Domain.Dtos.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        Task<List<AboutUsDto>> GetAll();
        Task<FilterContactUs> FilterContactUs(FilterContactUs filter);
        Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about);
        Task<EditAboutUsDto> GetAboutUsForEdit(long id);
        Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto edit, string username);
        Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);


        #region Ticket

        Task<AddTicketResult> AddUserTicket(AddTicketDto Ticket, long userId);
        Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId);
        Task<FilterTicketDto> TicketList(FilterTicketDto filter);
        Task<string?> GetUserAvatarTicket(long ticketId);
        Task<string?> GetAdminUserAvatarTicket(long ticketId);
        Task<AnswerTicketResult> AnswerTicket(AnswerTicketDto answer, long userId);
        Task<AnswerTicketResult> AdminAnswerTicket(AnswerTicketDto answer, long userId);
        Task<bool> CloseTicket(long ticketId);



        #endregion

    }
}
