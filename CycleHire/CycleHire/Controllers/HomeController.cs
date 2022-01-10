using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Core.ViewModels;
using CycleHire.Services;
using System.Threading.Tasks;
using CycleHire.Core.ViewModels.EmailTemplatesViewModels;
using System;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.ViewModels.HomeViewModels;
using CycleHire.Extensions;

namespace CycleHire.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Search(SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Home");
            }

            if (!model.Radius.ContainsKey(model.SelectedRadius))
            {
                model.DefaultRadius();
            }

            var radiusInKilometers = model.SelectedRadius.ToKilometers();

            var listings = _unitOfWork.Listings.FindByLatLngAsync(model.Latitude, model.Longitude, radiusInKilometers);

            model.Listings = listings;

            return View(model);
        }

        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View("~/Views/Error/Error500.cshtml");
        }
    }
}
