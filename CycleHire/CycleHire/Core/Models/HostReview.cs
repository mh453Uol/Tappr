using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class HostReview : Audit
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        [Required]
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Communication")]
        public byte ResponseAndCommunication { get; set; }

        [MaxLength(500)]
        [Display(Name = "Message")]
        public String ReviewMessage { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Accuracy")]
        public byte AccuracyOfListing { get; set; }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Location")]
        public byte Location { get; set; }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Value")]
        public byte Value { get; set; }
    }
}

