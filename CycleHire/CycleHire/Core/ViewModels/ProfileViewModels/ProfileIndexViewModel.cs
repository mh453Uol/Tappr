using CycleHire.Core.Models;
using CycleHire.Core.ViewModels.ReviewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ProfileViewModels
{
    public class ProfileIndexViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<HostReview> ReviewsAboutHosts { get; set; }
        public IEnumerable<TenantReview> ReviewsAboutTenants { get; set; }
    }
}
