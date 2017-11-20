using infrastructure.cqs;
using Microsoft.AspNetCore.Mvc;
using query.dto;
using query.messages.post;
using System.Collections.Generic;

namespace peruncore.Controllers
{
 

    public class HomeController : Controller
    {
        private IQueryHandler<GetAllPublishedPostsQuery, List<PostDTO>> _getAllPublishedPostsQuery;

        public HomeController(
            IQueryHandler<GetAllPublishedPostsQuery, List<PostDTO>> getAllPublishedPostsQuery
            )
        {
            _getAllPublishedPostsQuery = getAllPublishedPostsQuery;
        }

        public IActionResult Index()
        {
            var query = new GetAllPublishedPostsQuery() { isPublished = true};
            var dtos = _getAllPublishedPostsQuery.Handle(query);
            return View(dtos);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
