using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using command.messages.post;
using infrastructure.cqs;
using peruncore.Models.Post;

namespace peruncore.Controllers
{
    public class PostController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger _logger;

        public PostController(ICommandDispatcher commandDispatcher, ILogger<PostController> logger)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int id, string slug)
        {
            return View();
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

            return Content(command.CommandId + ";" + command.Title);
        }
    }
}