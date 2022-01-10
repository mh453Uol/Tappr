using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.HomeViewModels
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            Radius = new Dictionary<byte, string>
            {
                { 1, "Within 1 Mile" },
                { 2, "Within 2 Mile" },
                { 3, "Within 3 Mile" },
                { 5, "Within 5 Mile" },
                { 10, "Within 10 Mile" },
                { 15, "Within 15 Mile" },
                { 20, "Within 20 Mile" },
                { 25, "Within 25 Mile" },
            };

            DefaultRadius();
        }


        [Required]
        [Display(Name = "Location")]
        [StringLength(maximumLength: 300, MinimumLength = 1, ErrorMessage = "The {0} must be minimum {1} and maximum {2} characters long")]
        public String Location { get; set; }

        [Required]
        [Range(-85.05115, +85, ErrorMessage = "Latitude is not valid")]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, +180, ErrorMessage = "Longitude is not valid")]
        public decimal Longitude { get; set; }

        public Dictionary<byte, string> Radius { get; set; }
        public byte SelectedRadius { get; set; }

        public IEnumerable<Listing> Listings { get; set; }

        public void DefaultRadius()
        {
            SelectedRadius = 10;
        }
    }
}
