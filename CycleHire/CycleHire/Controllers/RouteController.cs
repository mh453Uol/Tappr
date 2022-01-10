using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CycleHire.Controllers
{
    [Authorize]
    public class RouteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}