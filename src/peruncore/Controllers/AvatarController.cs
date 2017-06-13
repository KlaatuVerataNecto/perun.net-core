using infrastucture.libs.image;
using Microsoft.AspNetCore.Mvc;
using peruncore.Models.User;
using System;
using System.IO;

namespace peruncore.Controllers
{
    public class AvatarController : Controller
    {
        [HttpPost]
        public ActionResult upload(UserAvatarModel model)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.avatar_image.CopyToAsync(stream);
            }
            // TODO: read image file path from config
            //Path.DirectorySeparatorChar
            var config = new ImageConfig().CreateConfigFromImageFile(filePath).WithSaveTo(@"C:\github\perun.net.core\src\peruncore\wwwroot\images\test.jpg");
            var image = ImageService.ResizeAndSave(config);

            return File(config.SavePathFile, "image/jpeg");
        }
    }
}