using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Entities.Contact.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class TicketDetailDto
    {
        public Entities.Contact.Ticket.Ticket Ticket { get; set; }
        public User Owner { get; set; }
        public List<TicketMessage> TicketMessage { get; set; }
    }
}
