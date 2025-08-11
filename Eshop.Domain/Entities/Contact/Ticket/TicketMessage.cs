using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Entities.Contact.Ticket
{
    public class TicketMessage : BaseEntity
    {
        #region Properties

        public long TicketId { get; set; }
        public long SenderId { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public String Text { get; set; }
        #endregion

        #region Relations
        public User Sender { get; set; }
        public Ticket Ticket { get; set; }
        #endregion
    }

}
