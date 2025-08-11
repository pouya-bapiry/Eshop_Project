using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class CloseTicketDto
    {
        public long Id { get; set; }
       
        public Eshop.Domain.Entities.Contact.Ticket.Ticket Ticket { get; set; }
    }
    public enum CloseTicketResult
    {
        Success,
        Error,
        NotFound
    }
}
