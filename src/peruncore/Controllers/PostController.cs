using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using command.messages.post;
using infrastructure.cqs;


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

        public IActionResult Index()
        {
            return View();
        }

        // Demonstration of how to use command dispatcher

        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreatePostCommand();
            command.CommandId = Guid.NewGuid();
            command.Title = "Hello";

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