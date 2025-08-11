using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Common;
using Eshop.Domain.Entities.Contact.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class FilterTicketDto:BasePaging

    { 
        public string Title { get; set; }
        public long? UserId { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public TicketState? TicketState { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public List<Entities.Contact.Ticket.Ticket> Tickets { get; set; }




        #region Methods

        public FilterTicketDto SetTickets(List<Entities.Contact.Ticket.Ticket> tickets)
        {
            Tickets = tickets;
            return this;
        }

        public FilterTicketDto SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            AllEntitiesCount = paging.AllEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

    public enum FilterTicketState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "درحال بررسی")]
        UnderProgress,


        [Display(Name = "بسته شده")]
        Closed,

        [Display(Name = "پاسخ داده شده")]
        Answered,


    }
    //public enum CloseTicketResult
    //{
    //    Success,
    //    Error,
    //    NotFound
    //}
    public enum FilterTicketOrder
    {
        CreateDateDescending,
        CreateDateAscending,
    }
}
