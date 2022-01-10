using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ReviewViewModels
{
    public abstract class ReviewViewModel
    {
        [Required]
        public Guid BookingId { get; set; }

        public DateTime BookingTo { get; set; }

        [Required]
        [Display(Name = "Communication")]
        [Range(0, 5, ErrorMessage = "You can only rate from 0 to 5")]
        public byte ResponseAndCommunication { get; set; }

        [MaxLength(500)]
        [Display(Name = "Review")]
        public String ReviewMessage { get; set; }

        public bool AbleToReview
        {
            get
            {
                DateTime validUntil = BookingTo.Date.AddDays(14);
                return DateTime.Now.Date > validUntil;
            }
        }
    }
}
