using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.ViewModels.ListingViewModels;
using AutoMapper;
using CycleHire.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CycleHire.Core.ViewModels;
using System.Collections.Generic;
using CycleHire.Services;
using CycleHire.Core.ViewModels.ReviewViewModels;

namespace CycleHire.Controllers
{
    public class ListingController : Controller
    {
        private readonly IImageUploader _imageUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListingController(IImageUploader imageUploader, IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _imageUploader = imageUploader;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        // GET: Listing
        public ActionResult Index()
        {
            return View();
        }

        // GET: Listing/Details/5
        public async Task<ActionResult> Details(Guid id)
        {

            var listing = await _unitOfWork.Listings.GetByIdWithImagesAndAccessories(id);

            if (listing == null)
            {
                return NotFound();
            }

            var reviews = await _unitOfWork.Reviews.GetByListingId(id);

            var model = _mapper.Map<ListingDetailsViewModel>(listing);

            model.Review = new HostReviewSummaryViewModel(reviews);

            return View(model);
        }

        [Authorize]
        // GET: Listing/Create
        public async Task<ActionResult> Create()
        {
            var model = new ListingForm
            {
                Accessories = _mapper.Map<List<KeyValueViewModel>>
                    (await _unitOfWork.Accessories.GetAllAsync())
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ListingForm model)
        {
            if (!ModelState.IsValid)
            {
                model.Accessories = _mapper.Map<List<KeyValueViewModel>>
                    (await _unitOfWork.Accessories.GetAllAsync());

                model.Availability.PopulateDaysOfWeek();

                return View(model);
            }

            var userId = _userManager.GetUserId(HttpContext.User);

            var images = _mapper.Map<List<ListingImage>>
                (await _imageUploader.UploadImagesAsync(model.UploadedImages));

            var listing = _mapper.Map<Listing>(model);

            listing.UserId = userId;
            listing.Images = images;

            await _unitOfWork.Listings.AddAsync(listing);

            _unitOfWork.Complete();

            _unitOfWork.Listings.SetSpatialLocation(listing.Latitude, listing.Longitude, listing.Id);

            return RedirectToAction("Details", new { id = listing.Id });
        }

        // GET: Listing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Listing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Listing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Listing/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}