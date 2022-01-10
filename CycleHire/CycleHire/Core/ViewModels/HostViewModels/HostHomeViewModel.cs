using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.HostViewModels
{
    public class HostHomeViewModel
    {
        public HostHomeViewModel(ApplicationUser user)
        {
            User = user;
            UserType = UserType.Host;
            IsHostStripeConnected = User.IsHostStripeConnected();
            PendingBookingRequests = new List<Booking>();
        }
        public ApplicationUser User { get; set; }
        public UserType UserType { get; set; }
        public bool IsHostStripeConnected { get; set; }
        public bool HasAddedListing { get; set; }
        public bool IsProfileComplete
        {
            get { return HasAddedListing && IsHostStripeConnected; }
        }
        public List<Booking> PendingBookingRequests { get; set; }
        public HostDeclineViewModel ToDeclineViewModel(Booking booking)
        {
            return new HostDeclineViewModel
            {
                BookingId = booking.Id,
                TenantName = booking.User.Firstname,
            };
        }
    }
}
