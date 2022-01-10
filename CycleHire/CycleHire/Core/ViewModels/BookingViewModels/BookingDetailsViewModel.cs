using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.BookingViewModels
{
    public class BookingDetailsViewModel
    {
        public Booking Booking { get; set; }

        public String CardLast4Digits { get; set; }

        public String CardType { get; set; }
        public bool IsUser { get; set; }
        public bool IsHost { get; set; }
        public String ErrorMessage { get; set; }
    }
}
