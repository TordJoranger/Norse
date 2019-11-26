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
        public ActionResult<Route> Index(string start, string destination)
        {
            if (start == null || destination == null)
            {
                return BadRequest();
            }

            return _service.GetRoute(start, destination);
        }

    }
}