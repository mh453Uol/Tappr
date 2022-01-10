using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Dtos
{
    public class FullCalendarEventDto
    {

        public FullCalendarEventDto(Guid id, String title, DateTime start, DateTime end)
        {
            Id = id;
            Title = title;
            Start = start;
            //FulllCalendar uses exclusive end dates
            End = end.AddDays(1);
            AllDay = true;
            URL = "/Booking/Details/" + id;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string URL { get; set; }
    }
}
