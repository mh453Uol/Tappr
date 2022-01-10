using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class Listing : Audit
    {
        public Listing()
        {
            Accessories = new Collection<ListingAccessory>();
            Images = new Collection<ListingImage>();
        }
        public Guid Id { get; set; }

        [Required]
        public String UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(50)]
        public String Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public String Description { get; set; }

        [Required]
        public ListingType ListingType { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(300)]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,6)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal Longitude { get; set; }

        public Availability Availability { get; set; }
        public ICollection<ListingAccessory> Accessories { get; set; }
        public ICollection<ListingImage> Images { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
