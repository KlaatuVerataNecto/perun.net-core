//using System;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

//namespace peruncore.Controllers
//{
//    public class AvatarController : Controller
//    {
//        [HttpPost]
//        [Authorize]
//        public ActionResult upload(AvatarUploadViewModel model)
//        {
//            // Get user id
//            int user_id = int.Parse(User.Identity.GetUserId());
//            bool is_exception = false;

//            // save image
//            // TODO: can be made a bit nicer
//            try
//            {
//                var files = Request.Files;

//                string filename = ImageService.cropAndSave(
//                    files,
//                    AppSettings.ImageUploadMaxSize,
//                    AppSettings.ImageAvatarWidth,
//                    AppSettings.ImageAvatarWidth,
//                    model.avatar_width,
//                    model.avatar_height,
//                    model.avatar_x,
//                    model.avatar_y,
//                    AppSettings.ImageAvatarPath,
//                    user_id);

//                // Save filename in database 
//                _userAccountService.update_profile_picture(user_id, filename);

//                var login = _userAuthRepository.find_user_by_id(user_id);
//                IdentitySignIn.sign_in(login.Id, login.Username, login.Roles, login.Avatar);

//            }
//            catch (FileNotFoundException ex)
//            {
//                is_exception = true;
//                ex.Ship("API_KEY", new Guid("LOG_ID"), HttpContext);
//            }
//            catch (ImageFileSizeTooBigException ex)
//            {
//                is_exception = true;
//                ex.Ship("API_KEY", new Guid("LOG_ID"), HttpContext);
//            }
//            catch (ImageInavlidAttributeException ex)
//            {
//                is_exception = true;
//                ex.Ship("API_KEY", new Guid("LOG_ID"), HttpContext);
//            }
//            catch (ImageInvalidCropAreaException ex)
//            {
//                is_exception = true;
//                ex.Ship("API_KEY", new Guid("LOG_ID"), HttpContext);
//            }
//            catch (Exception ex)
//            {
//                is_exception = true;
//                ex.Ship("API_KEY", new Guid("LOG_ID"), HttpContext);
//            }

//            if (is_exception)
//            {
//                ModelState.AddModelError("email", UserResponseMessagesResource.error_wierd_shit_going_on);
//                TempData["ModelState"] = ModelState;
//            }

//            return RedirectToAction("avatar", "account", new { id = user_id });
//        }
//    }
//}