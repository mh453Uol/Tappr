using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ReviewViewModels
{
    public class TenantReviewViewModel:ReviewViewModel
    {
        [Required]
        [Display(Name = "Mindfull of Bike")]
        public byte LookedAfterBike { get; set; }
    }
}
