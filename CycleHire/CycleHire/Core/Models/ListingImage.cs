using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class ListingImage : Audit
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImagePublicId { get; set; }

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
    }
}
