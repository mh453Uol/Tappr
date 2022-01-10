using System;
using System.Collections.Generic;

namespace CycleHire.Core.Models
{
    public class Availability
    {
        public int Id { get; set; }

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wenesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public int[] GetDaysNotAvailable()
        {
            List<int> days = new List<int>();

            if (!Sunday) { days.Add(1); };
            if (!Monday) { days.Add(2); };
            if (!Tuesday) { days.Add(3); }
            if (!Wenesday) { days.Add(4); }
            if (!Thursday) { days.Add(5); }
            if (!Friday) { days.Add(6); }
            if (!Saturday) { days.Add(7); }

            return days.ToArray();
        }
    }
}
