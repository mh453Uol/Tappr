using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ReviewViewModels
{
    public class ReviewIndexViewModel
    {
        public ReviewIndexViewModel()
        {
            UserType = UserType.Tenant;
            PendingReviewsAboutHost = new List<Booking>();
            PendingReviewsAboutTenant = new List<Booking>();
            ReviewsAboutHosts = new List<HostReview>();
            ReviewsAboutTenants = new List<TenantReview>();
        }
        public UserType UserType { get; set; }
        public IEnumerable<HostReview> ReviewsAboutHosts { get; set; }
        public IEnumerable<TenantReview> ReviewsAboutTenants { get; set; }
        public IEnumerable<Booking> PendingReviewsAboutHost { get; set; }
        public IEnumerable<Booking> PendingReviewsAboutTenant { get; set; }
    }
}
