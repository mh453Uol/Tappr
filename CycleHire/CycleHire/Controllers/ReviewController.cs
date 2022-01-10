using Microsoft.AspNetCore.Mvc;
using CycleHire.Persistence.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CycleHire.Core.Models;
using System.Threading.Tasks;
using System;
using CycleHire.Core.ViewModels.ReviewViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CycleHire.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Pending()
        {
            var userId = _userManager.GetUserId(User);

            var model = new ReviewIndexViewModel();

            if (User.IsInRole("Host"))
            {
                model.UserType = UserType.Host;
                model.PendingReviewsAboutTenant = await _unitOfWork.Reviews
                    .GetPendingReviewsAboutTenant(userId);
            }

            model.PendingReviewsAboutHost = await _unitOfWork.Reviews
                .GetPendingReviewsAboutHost(userId);

            return View(model);
        }

        public async Task<IActionResult> Created()
        {
            var userId = _userManager.GetUserId(User);

            var model = new ReviewIndexViewModel();

            if (User.IsInRole("Host"))
            {
                model.UserType = UserType.Host;

                model.ReviewsAboutTenants = await _unitOfWork.Reviews
                    .GetTenantReviewsByUserId(userId);
            }

            model.ReviewsAboutHosts = await _unitOfWork.Reviews
                .GetHostReviewsByUserId(userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Host(Guid id)
        {
            var booking = await _unitOfWork.Bookings.GetById(id);

            var userId = _userManager.GetUserId(User);

            var isUser = IsTenantOfBooking(booking, userId);

            if (!isUser) { return Forbid(); }

            var model = new HostReviewViewModel
            {
                BookingId = booking.Id,
                BookingTo = booking.To
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Host(HostReviewViewModel model)
        {
            var booking = await _unitOfWork.Bookings.GetById(model.BookingId);

            if (!ModelState.IsValid)
            {
                model.BookingId = booking.Id;
                model.BookingTo = booking.To;
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (!IsTenantOfBooking(booking, userId)) { return Forbid(); }

            var review = _mapper.Map<HostReview>(model);
            review.UserId = userId;
            review.ListingId = booking.ListingId;

            _unitOfWork.Reviews.AddHostReview(review);
            _unitOfWork.Complete();

            return RedirectToAction("Created");
        }

        [HttpGet]
        public async Task<IActionResult> Tenant(Guid id)
        {
            var booking = await _unitOfWork.Bookings.GetById(id);

            var userId = _userManager.GetUserId(User);

            if (!IsHostOfBooking(booking, userId)) { return Forbid(); }

            var model = new TenantReviewViewModel
            {
                BookingId = booking.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Tenant(TenantReviewViewModel model)
        {
            var booking = await _unitOfWork.Bookings.GetById(model.BookingId);

            if (!ModelState.IsValid)
            {
                model.BookingId = booking.Id;
                model.BookingTo = booking.To;
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (!IsHostOfBooking(booking, userId)) { return Forbid(); }

            var review = _mapper.Map<TenantReview>(model);
            review.UserId = userId;
            review.ListingId = booking.ListingId;

            _unitOfWork.Reviews.AddTenantReview(review);
            _unitOfWork.Complete();

            return RedirectToAction("Created");
        }


        public bool IsHostOfBooking(Booking booking, string userId)
        {
            return booking.OwnerId == userId;
        }

        public bool IsTenantOfBooking(Booking booking, string userId)
        {
            return booking.UserId == userId;
        }
    }
}