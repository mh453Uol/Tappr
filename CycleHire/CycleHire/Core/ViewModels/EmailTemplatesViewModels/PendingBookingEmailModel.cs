using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.EmailTemplatesViewModels
{
    public class PendingBookingEmailModel
    {
        public string TenantName { get; set; }
        public string ListingTitle { get; set; }
        public string ListingImage { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Duration
        {
            get { return (To - From).Days + 1; }
        }
        public decimal HostEarning { get; set; }
        public string BookingLink { get; set; }
    }
}
