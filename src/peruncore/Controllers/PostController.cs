using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;

using command.messages.post;
using infrastructure.cqs;
using peruncore.Models.Post;
using query.messages.post;
using query.dto;
using query.dto.common;
using peruncore.Config;
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
        private readonly IMapper _mapper;

        public PostController(
            IQueryHandler<GetPostByGuidQuery, DTO> getPostByGuidQuery,
            IQueryHandler<GetPostByIdQuery, PostDTO> getPostByIdQuery,
            ICommandDispatcher commandDispatcher,
            IMapper mapper,
            IImageService imageService,
            IHostingEnvironment hostingEnvironment,
            IOptions<ImageUploadSettings> imageUploadSettings,
            ILogger<PostController> logger
            )
        {
            _getPostByGuidQuery = getPostByGuidQuery;
            _getPostByIdQuery = getPostByIdQuery;
            _commandDispatcher = commandDispatcher;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _imageUploadSettings = imageUploadSettings.Value;
            _imageService = imageService;
            _logger = logger;
        }

        [Route("post/{id:int}/{slug?}")]
        public IActionResult Index(int id, string slug, string thanks)
        {
            // query post by id and url slug
            var post = _getPostByIdQuery.Handle(new GetPostByIdQuery { PostId = id });

            if (post != null)
            {
                return View(new PostViewModel(post.title, _imageUploadSettings.PostImageDirURL + post.postimage, !String.IsNullOrEmpty(thanks)));
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
            // render upload form
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostModel model)
        {   
            // Validate if previously uploaded image is the same as the one in the post 
            if (TempData["uploaded_image"] == null || TempData["uploaded_image"].ToString() != model.post_image)
            {
                _logger.LogError("Post image filename uploaded doesn't correspond to one in session 'uploaded_image' ");
                TempData["uploaded_image"] = null;
                return RedirectToAction("Index", "Error");
            }

            TempData["uploaded_image"] = null;

            // Create command using Automapper
            var command = _mapper.Map<CreatePostCommand>(model);

            try
            {
                // dispatch command
                _commandDispatcher.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
                return RedirectToAction("Index", "Error");
            }

            // query created  post
            var post = _getPostByGuidQuery.Handle(new GetPostByGuidQuery { Guid = command.CommandId });

            if (post != null)
            {
                // redirect to created post using post id and url slug
                return RedirectToAction("Index", new { id = post.id, slug = post.urlSlug, thanks = 1 });
            }
            else
            {
                // command has failed to create
                _logger.LogError("Post not created after dispathcing command:", command);
                return RedirectToAction("Index","Error");
            }
        }
    }
}