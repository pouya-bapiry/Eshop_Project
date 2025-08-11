using Eshop.Application.Extensions;
using Eshop.Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceHost.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload, string ckEditorFuncName, string ckEditor, string langCode)
        {
            if (upload.Length <= 0)
            {
                return null;
            }

            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب نمایید";
                var notImage = JsonConvert.DeserializeObject("{'uploaded' : 0, 'error': {'message': \" " + notImageMessage + " \"}}");

                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName, PathExtension.UploaderImageServer, null, null);

            return Json(new
            {
                upload = true,
                Url = $"{PathExtension.UploaderImage}{fileName}",

            });


        }
    }
}
