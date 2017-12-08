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

namespace peruncore.Controllers
{
    public class PostController : Controller
    {
        private IQueryHandler<GetPostByGuidQuery, DTO> _getPostByGuidQuery;
        private IQueryHandler<GetPostByIdQuery, PostDTO> _getPostByIdQuery;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger _logger;

        public PostController(
            IQueryHandler<GetPostByGuidQuery, DTO> getPostByGuidQuery,
            IQueryHandler<GetPostByIdQuery, PostDTO> getPostByIdQuery,
            ICommandDispatcher commandDispatcher, 
            ILogger<PostController> logger)
        {
            _getPostByGuidQuery = getPostByGuidQuery;
            _getPostByIdQuery = getPostByIdQuery;
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int id, string slug)
        {
            var post = _getPostByIdQuery.Handle(new GetPostByIdQuery { PostId = id });

            if (post != null)
            {
                // TODO: Create view
                return View(post);
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
            //TODO: Use Automapper
            var command = new CreatePostCommand();
            command.CommandId = Guid.NewGuid();
            command.Title = model.title;
            command.ImageName = model.post_image.FileName;

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