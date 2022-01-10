using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using CycleHire.Services;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.Models;
using CycleHire.Core.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Http;
using CycleHire.Core.ViewModels;
using CycleHire.Core.ViewModels.ManageViewModels;

namespace CycleHire.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IImageUploader _imageUploader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IUnitOfWork unitOfWork, IMapper mapper,
            IImageUploader imageUploader, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _imageUploader = imageUploader;
        }
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) { return View(user); }

            user.Listings = await _unitOfWork.Listings.GetByUserIdWithImages(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IndexViewModel model)
        {
            if (model?.UploadImageModel?.Image == null)
            {
                return RedirectToAction("Index", "Manage");
            }

            var user = await _userManager.GetUserAsync(User);

            var imageId = await _imageUploader
                .UploadImageAsync(model.UploadImageModel.Image);

            user.ProfileImageId = imageId;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Manage");
        }
    }
}