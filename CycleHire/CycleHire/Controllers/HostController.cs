using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CycleHire.Core.Models;
using CycleHire.Core.ViewModels.HostViewModels;
using CycleHire.Persistence.UnitOfWork;

namespace CycleHire.Controllers
{
    [Authorize(Roles = "Host")]
    public class HostController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public HostController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // GET: Host
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var lisitings = await _unitOfWork.Listings.GetCountByUserId(user.Id);

            var model = new HostHomeViewModel(user);

            if (lisitings == 0)
            {
                return View(model);
            }

            model.HasAddedListing = true;

            model.PendingBookingRequests = await _unitOfWork.Host.GetPendingBookings(user.Id);

            return View(model);
        }

        public async Task<IActionResult> Listings()
        {
            var userId = _userManager.GetUserId(User);
            var listings = await _unitOfWork.Listings.GetByUserIdWithImages(userId);

            return View(listings);
        }

        public IActionResult Bookings()
        {
            var userId = _userManager.GetUserId(User);
            return View();
        }
    }
}