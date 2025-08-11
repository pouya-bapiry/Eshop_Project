using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
  [Authorize("AdminArea")]
    [Area("Administration")]
    [Route("administration")]
    public class AdminBaseController : Controller
    {
        protected String ErrorMessage = "ErrorMessage";
        protected String SuccessMessage = "SuccessMessage";
        protected String InfoMessage = "InfoMessage";
        protected String WarningMessage = "WarningMessage";
    }
}
