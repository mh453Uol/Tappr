using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Core.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using CycleHire.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using CycleHire.Core.ViewModels.BookingViewModels;
using CycleHire.Core.ViewModels.ListingViewModels;
using System;
using Stripe;
using CycleHire.Services;
using CycleHire.Core.ViewModels.HostViewModels;
using System.Linq;

namespace CycleHire.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStripeService _stripe;
        private readonly IEmailSender _emailSender;
        private readonly IImageUploader _imageUploader;

        private readonly UserManager<ApplicationUser> _userManager;
        public BookingController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager, IStripeService stripe,
            IEmailSender emailSender, IImageUploader imageUploader)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _stripe = stripe;
            _emailSender = emailSender;
            _imageUploader = imageUploader;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var bookings = await _unitOfWork.Bookings.GetByUserIdWithListingAndOwner(userId);

            return View(bookings);
        }

        // GET: Booking/Create
        [HttpGet]
        public async Task<IActionResult> Create(ListingDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Listing", new { id = model.Id });
            }

            var listing = await _unitOfWork.Listings.GetByIdWithAccessories(model.Id);

            var available = await _unitOfWork.Bookings.IsAvailable(listing.Id, model.From.Value, model.To.Value);

            if (!available)
            {
                return RedirectToAction("Details", "Listing", new { id = model.Id });
            }

            var user = await _userManager.GetUserAsync(User);

            BookingFormViewModel viewModel = new BookingFormViewModel
            {
                Listing = _mapper.Map<ListingDetailsViewModel>(listing)
            };

            viewModel.PricePerDay = viewModel.Listing.Price;
            viewModel.Listing.From = model.From.Value;
            viewModel.Listing.To = model.To.Value;
            viewModel.UserEmailAddress = user.Email;
            viewModel.CalculateTotalPrice();

            return View(viewModel);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var listing = await _unitOfWork.Listings.GetByIdWithImagesAndAccessories(model.Listing.Id);

            var available = await _unitOfWork.Bookings.IsAvailable(listing.Id,
                model.Listing.From.Value, model.Listing.To.Value);

            if (!available) { return NotFound(); }

            var stripeCustomerId = await _stripe.AddCustomerToStripeAsync(model.StripeToken, user.Email);

            var booking = _mapper.Map<Booking>(model);

            _mapper.Map(listing, booking);

            booking.User = user;
            booking.StripeCustomerIdToken = stripeCustomerId;
            booking.CalculateTotalPrice();
            booking.NewlyCreated();

            await _unitOfWork.Bookings.AddAsync(booking);

            _unitOfWork.Complete();

            var bookingUrl = Url.NewBookingCallbackLink(listing.Id, Request.Scheme);
            var imageUrl = _imageUploader.GetBikeImageById(listing.Images.FirstOrDefault()?.ImagePublicId, 690, 690);
            await _emailSender.SendNewBookingEmailAsync(listing.User.Email, bookingUrl, imageUrl, booking);

            return RedirectToAction("Details", new { id = booking.Id });
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(Guid id, string message = null)
        {
            var booking = await _unitOfWork.Bookings.GetByIdWithTenant(id);

            var userId = _userManager.GetUserId(User);

            var isUser = IsTenantOfBooking(booking, userId);
            var isHost = IsHostOfBooking(booking, userId);

            if (!isUser && !isHost) { return Forbid(); }

            booking.Listing = await _unitOfWork.Listings.GetByIdWithAccessories(booking.ListingId);

            var model = _mapper.Map<BookingDetailsViewModel>(booking);
            model.IsUser = isUser;
            model.IsHost = isHost;
            model.ErrorMessage = message;

            model.CardLast4Digits = await _stripe.
                GetCustomerCardDetailsAsync(booking.StripeCustomerIdToken);

            return View(model);
        }

        [Authorize(Roles = "Host")]
        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var booking = await _unitOfWork.Bookings.GetById(id);

            if (!booking.IsOwnerOfBooking(user.Id) && !booking.IsNotExpired()) { return Unauthorized(); }

            var isAvailable = await _unitOfWork.Bookings.IsAvailable(booking.Id, booking.To, booking.From);

            if (!isAvailable) { return NotFound(); }

            int amountInPennies = (int)(booking.TotalPrice * 100);

            var success = await _stripe.ProcessPayment(booking.StripeCustomerIdToken, amountInPennies, user.Email, "Tapr Booking");

            if (!success)
            {
                return RedirectToAction("Detail", "Booking", new { Id = id, message = "We could not process your card details" });
            }

            booking.Approve();

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Host", null);
        }

        [Authorize(Roles = "Host")]
        [HttpPost]
        public async Task<IActionResult> Decline(HostDeclineViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var booking = await _unitOfWork.Bookings.GetById(model.BookingId);

            if (!booking.IsOwnerOfBooking(userId) && !booking.IsNotExpired()) { return Unauthorized(); }

            booking.Decline(model?.Message);

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Host", null);
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var booking = await _unitOfWork.Bookings.GetById(id);

            var userId = _userManager.GetUserId(User);

            if (!IsTenantOfBooking(booking, userId)) { return Forbid(); }

            booking.CancelByTenant();

            _unitOfWork.Complete();

            return RedirectToAction("Index");
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