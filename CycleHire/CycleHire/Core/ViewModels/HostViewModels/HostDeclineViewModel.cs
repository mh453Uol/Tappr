using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.HostViewModels
{
    public class HostDeclineViewModel
    {
        public Guid BookingId { get; set; }
        public string Message { get; set; }
        public string TenantName { get; set; }
    }
}
