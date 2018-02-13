using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using command.messages.post;
using infrastructure.cqs;
using peruncore.Models.Post;
using query.messages.post;
using query.dto;
using query.dto.common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using peruncore.Config;
using Microsoft.Extensions.Options;
using infrastucture.libs.image;

namespace peruncore.Controllers
{
    public class PostController : Controller
    {
        private IQueryHandler<GetPostByGuidQuery, DTO> _getPostByGuidQuery;
        private IQueryHandler<GetPostByIdQuery, PostDTO> _getPostByIdQuery;       
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IImageService _imageService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ImageUploadSettings _imageUploadSettings;
        private readonly ILogger _logger;

        public PostController(
            IQueryHandler<GetPostByGuidQuery, DTO> getPostByGuidQuery,
            IQueryHandler<GetPostByIdQuery, PostDTO> getPostByIdQuery,
            ICommandDispatcher commandDispatcher,
            IImageService imageService,
            IHostingEnvironment hostingEnvironment,
            IOptions<ImageUploadSettings> imageUploadSettings,
            ILogger<PostController> logger)
        {
            _getPostByGuidQuery = getPostByGuidQuery;
            _getPostByIdQuery = getPostByIdQuery;
            _commandDispatcher = commandDispatcher;
            _hostingEnvironment = hostingEnvironment;
            _imageUploadSettings = imageUploadSettings.Value;
            _imageService = imageService;
            _logger = logger;
        }

        [Route("post/{id:int}")]
        public IActionResult Index(int id, string slug)
        {
            var post = _getPostByIdQuery.Handle(new GetPostByIdQuery { PostId = id });

            if (post != null)
            {
                // TODO: Create view
                return View(new PostViewModel(post.title, _imageUploadSettings.PostImageDirURL + post.postimage));
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
          
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostModel model)
        {    
            if (TempData["uploaded_image"] != model.post_image)
            {
                _logger.LogError("Post image filename uploaded doesn't correspond to one in session 'uploaded_image' ");
                TempData["uploaded_image"] = null;
                return RedirectToAction("Index", "Error");
            }
            TempData["uploaded_image"] = null;

            //TODO: Use Automapper
            var command = new CreatePostCommand();
            command.CommandId = Guid.NewGuid();
            command.Title = model.title;
            command.ImageName = model.post_image;

            try
            {
                _commandDispatcher.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
                return RedirectToAction("Index", "Error");
            }

            var post = _getPostByGuidQuery.Handle(new GetPostByGuidQuery { Guid = command.CommandId });

            if (post != null)
            {
                return RedirectToAction("Index", new { id = post.id, slug = post.urlSlug });
            }
            else
            {
                _logger.LogError("Post not created after dispathcing command:", command);
                return RedirectToAction("Index","Error");
            }
        }
    }
}