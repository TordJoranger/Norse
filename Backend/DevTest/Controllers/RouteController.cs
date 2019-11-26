using Microsoft.AspNetCore.Mvc;
using DevTest.Models;
using DevTest.Services;

namespace DevTest.Controllers
{
    [Route("/route")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _service;

        public RouteController(IRouteService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<Route> Index(string from, string to)
        {
            if (from == null || to == null)
            {
                return BadRequest();
            }

            return _service.GetRoute(from, to);
        }

    }
}