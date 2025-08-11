using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entities.Contact;
using Eshop.Domain.Entities.Contact.Ticket;
using Eshop.Domain.Entities.Site;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Implementation
{
    public class ContactService : IContactService
    {

        #region Fields and Ctor
        private readonly IGenericRepository<ContactUs> _contactRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;
        private readonly IGenericRepository<AboutUs> _aboutUsRepository;

        public ContactService(IGenericRepository<ContactUs> contactRepository,
            IGenericRepository<Ticket> ticketRepository,
            IGenericRepository<TicketMessage> ticketMessageRepository,
            IGenericRepository<AboutUs> aboutUsRepository)
        {
            _contactRepository = contactRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _aboutUsRepository = aboutUsRepository;
        }




        #endregion



        #region Methods



        #region ContactUs
        public async Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId)
        {
            // todo : Use Sanitizer to sanitize input data

            var newContact = new ContactUs
            {
                UserId = (userId != null && userId.Value != 0) ? userId.Value : (long?)null,
                UserIp = userIp,
                Email = contact.Email,
                Fullname = contact.Fullname,
                MessageSubject = contact.MessageSubject,
                MessageText = contact.MessageText,
            };

            await _contactRepository.AddEntity(newContact);
            await _contactRepository.SaveChanges();
        }


        public async Task<FilterContactUs> FilterContactUs(FilterContactUs filter)
        {
            var query = _contactRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.User)
                .OrderByDescending(x => x.Id);

            #region Filter
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{filter.Email}%")).OrderByDescending(x => x.CreateDate);
            }


            #endregion

            #region Paging

            var contactCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, contactCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetContactUs(allEntities);

        }
        #endregion








        #region AboutUs

        #region Get
        public async Task<List<AboutUsDto>> GetAll()
        {
            return await _aboutUsRepository.GetQuery().AsQueryable().Select(x => new AboutUsDto
            {
                Id = x.Id,
                HeaderTitle = x.HeaderTitle,
                Description = x.Description,
                CreateDate = x.CreateDate.ToStringShamsiDate(),
                LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()

            }).ToListAsync();
        }
        #endregion

        #region Create
        public async Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about)
        {
            var newAboutUs = new AboutUs()
            {
                Description = about.Description,
                HeaderTitle = about.HeaderTitle,
            };
            _aboutUsRepository.AddEntity(newAboutUs);
            _aboutUsRepository.SaveChanges();

            return CreateAboutUsResult.Success;

        }
        #endregion

        #region Edit
        public async Task<EditAboutUsDto> GetAboutUsForEdit(long id)
        {
            var about = await _aboutUsRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return null;
            }
            return new EditAboutUsDto
            {
                Id = about.Id,
                HeaderTitle = about.HeaderTitle,
                Description = about.Description,

            };
        }

        public async Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto edit, string username)
        {
            var about = await _aboutUsRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (about == null)
            {
                return EditAboutUsResult.NotFound;

            }
            about.HeaderTitle = edit.HeaderTitle;
            about.Description = edit.Description;

            _aboutUsRepository.EditEntityByUser(about, username);

            _aboutUsRepository.SaveChanges();

            return EditAboutUsResult.Success;

        }
        #endregion

        #endregion




        #region Ticket


        public async Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId)
        {
            if (string.IsNullOrWhiteSpace(ticket.Text))
            {
                return AddTicketResult.Error;
            }

            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = ticket.Title,
                IsReadByOwner = true,
                TicketSection = ticket.TicketSection,
                TicketPriority = ticket.TicketPriority,
                TicketState = TicketState.UnderProgress,
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();


            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                SenderId = userId,
                Text = ticket.Text,
            };

            await _ticketMessageRepository.AddEntity(newMessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        public async Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId)
        {
            var ticket = await _ticketRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Owner)
                .OrderByDescending(x => x.CreateDate)
                .SingleOrDefaultAsync(x => x.Id == ticketId && !x.IsDelete);

            var ticketMessage = await _ticketMessageRepository
                .GetQuery()
                .AsQueryable()
                .Include(x=> x.Sender)
                .Include(x => x.Ticket)           
                .ThenInclude(x => x.Owner)
                .Where(x => x.TicketId == ticketId && !x.IsDelete)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
            //var sender = await _ticketMessageRepository.GetQuery().AsQueryable().Include(x => x.Sender).Where(x => x.TicketId == ticketId && !x.IsDelete)
            //    .OrderByDescending(x => x.CreateDate)
            //    .ToListAsync();

            if (ticket == null || ticket.OwnerId != userId)
            {
                return null;
            }

            return new TicketDetailDto
            {
                Ticket = ticket,
                TicketMessage = ticketMessage,
                Owner = ticket.Owner
            };


        }

        public async Task<FilterTicketDto> TicketList(FilterTicketDto filter)
        {


            var query = _ticketRepository
                .GetQuery()
                .Include(x => x.Owner)
                .AsQueryable();

            #region State

            switch (filter.TicketState)
            {
                case TicketState.All:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case TicketState.UnderProgress:
                    query = query.Where(x => x.TicketState == TicketState.UnderProgress && !x.IsDelete);
                    break;
                case TicketState.Answered:
                    query = query.Where(x => x.TicketState == TicketState.Answered && !x.IsDelete);
                    break;
                case TicketState.Closed:
                    query = query.Where(x => x.TicketState == TicketState.Closed && !x.IsDelete);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDateAscending:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
                case FilterTicketOrder.CreateDateDescending:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
            }

            #endregion

            #region Filter

            if (filter.TicketSection != null)
            {
                query = query.Where(x => x.TicketSection == filter.TicketSection.Value);
            }

            if (filter.TicketPriority != null)
            {
                query = query.Where(x => x.TicketPriority == filter.TicketPriority.Value);
            }

            if (filter.TicketState != null)
            {
                query = query.Where(x => x.TicketState == filter.TicketState.Value);
            }

            if (filter.UserId != null && filter.UserId != 0)
            {
                query = query.Where(x => x.OwnerId == filter.UserId.Value);
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));
            }

            #endregion

            #region Paging

            var ticketCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, ticketCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetTickets(allEntities);
            #endregion

        }

        public async Task<string?> GetUserAvatarTicket(long ticketId)
        {
            var ownerAvatar = await _ticketRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == ticketId);
            return ownerAvatar?.Owner.Avatar;
        }

        public async Task<string?> GetAdminUserAvatarTicket(long ticketId)
        {
            var adminAvatar = await _ticketMessageRepository
                 .GetQuery()
                 .AsQueryable()
                 .Include(x => x.Sender)
                 .FirstOrDefaultAsync(x => x.Id == ticketId);


            return adminAvatar?.Sender.Avatar;
        }
        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketDto answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null)
            {
                return AnswerTicketResult.NotFound;
            }

            if (ticket.OwnerId != userId)
            {
                return AnswerTicketResult.NotForUser;
            }
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text,
            };
            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();


            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            ticket.TicketState = TicketState.UnderProgress;
            await _ticketMessageRepository.SaveChanges();
            return AnswerTicketResult.Success;

        }

        public async Task<AnswerTicketResult> AdminAnswerTicket(AnswerTicketDto answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null)
            {
                return AnswerTicketResult.NotFound;
            }
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text,
            };
            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByAdmin = true;
            ticket.IsReadByOwner = false;
            ticket.TicketState = TicketState.Answered;
            await _ticketMessageRepository.SaveChanges();
            return AnswerTicketResult.Success;

        }
        public async Task<bool> CloseTicket(long ticketId)
        {
            var ticket = await _ticketRepository.GetEntityById(ticketId);


            if (ticketId == null)
            {
                return false;
            }

            ticket.TicketState = TicketState.Closed;

            _ticketRepository.EditEntity(ticket);
            await _ticketRepository.SaveChanges();

            return true;
        }

        #endregion



       



        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_contactRepository != null)
            {
                await _contactRepository.DisposeAsync();
            }
            if (_ticketRepository != null)
            {
                await _ticketRepository.DisposeAsync();
            }
            if (_ticketMessageRepository != null)
            {
                await _ticketMessageRepository.DisposeAsync();
            }
        }











        #endregion


    }
}
