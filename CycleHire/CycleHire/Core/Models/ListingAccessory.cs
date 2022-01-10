using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class ListingAccessory : Audit
    {
        [Required]
        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        [Required]
        public Guid AccessoryId { get; set; }
        public Accessory Accessory { get; set; }
    }
}
