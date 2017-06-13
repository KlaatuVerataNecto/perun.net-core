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
                ImageService.Save(stream, @"C:\", Guid.NewGuid());
            }
            return Content("ok");
        }
    }
}