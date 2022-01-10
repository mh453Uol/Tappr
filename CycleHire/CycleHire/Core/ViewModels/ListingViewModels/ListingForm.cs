using CycleHire.Core.Models;
using CycleHire.Utilites.Validators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CycleHire.Core.ViewModels.ListingViewModels
{
    public class ListingForm
    {
        public ListingForm()
        {
            Accessories = new List<KeyValueViewModel>();
            Availability = new AvailabilityViewModel();
            UploadedImages = new List<IFormFile>();
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "  The {0} must be maximum {2} characters long")]
        public String Title { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "  The {0} must be maximum {2} characters long")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Hire price per day (£)")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(maximumLength: 300, MinimumLength = 1, ErrorMessage = "The {0} must be minimum {1} and maximum {2} characters long")]
        public String Address { get; set; }

        [Required]
        [Range(-85.05115, +85, ErrorMessage = "Latitude is not valid")]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, +180, ErrorMessage = "Longitude is not valid")]
        public decimal Longitude { get; set; }

        public AvailabilityViewModel Availability
        { get; set; }
        public Guid[] IncludedAccessories { get; set; }
        public List<KeyValueViewModel> Accessories { get; set; }

        [Display(Name = "Uploaded Image")]
        [MinimumItem(1, ErrorMessage = "Please upload at least one image")]
        public List<IFormFile> UploadedImages { get; set; }
    }
}
