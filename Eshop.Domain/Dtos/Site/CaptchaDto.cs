using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Dtos.Site
{
    public class CaptchaDto
    {
            [Display(Name = "من ربات نیستم")]
            [Required(ErrorMessage = "لطفا تیک {0} را بزنید")]
            public string Captcha { get; set; }
        
    }
}
