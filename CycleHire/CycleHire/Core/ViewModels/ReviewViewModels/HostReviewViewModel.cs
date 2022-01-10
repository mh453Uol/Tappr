using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ReviewViewModels
{
    public class HostReviewViewModel : ReviewViewModel
    {
        //We are reviewing the Host
        [Required]
        [Display(Name = "Accuracy of Listing")]
        [Range(0, 5, ErrorMessage = "You can only rate from 0 to 5")]
        public byte AccuracyOfListing { get; set; }

        [Required]
        [Display(Name = "Location")]
        [Range(0, 5, ErrorMessage = "You can only rate from 0 to 5")]
        public byte Location { get; set; }

        [Required]
        [Display(Name = "Value")]
        [Range(0, 5, ErrorMessage = "You can only rate from 0 to 5")]
        public byte Value { get; set; }
    }
}
