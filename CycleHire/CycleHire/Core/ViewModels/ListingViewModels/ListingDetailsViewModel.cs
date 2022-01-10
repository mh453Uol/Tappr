using CycleHire.Core.Models;
using CycleHire.Core.ViewModels.AccessoryViewModel;
using CycleHire.Core.ViewModels.ReviewViewModels;
using CycleHire.Utilites.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ListingViewModels
{

    public class ListingDetailsViewModel
    {
        public ListingDetailsViewModel()
        {
            Accessories = new List<KeyValueViewModel>();
            User = new ApplicationUserViewModel();
        }

        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Price { get; set; }
        public ICollection<KeyValueViewModel> Accessories { get; set; }
        public IEnumerable<string> Images { get; set; }
        public ApplicationUserViewModel User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [LessThan("To")]
        public DateTime? From { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Created { get; set; }

        public HostReviewSummaryViewModel Review { get; set; }
    }
}
