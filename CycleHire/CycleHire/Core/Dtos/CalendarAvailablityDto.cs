using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Dtos
{
    public class CalendarAvailablityDto
    {
        public IEnumerable<DateTime> BookedDates { get; set; }
        public int[] NotRentableDays;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
