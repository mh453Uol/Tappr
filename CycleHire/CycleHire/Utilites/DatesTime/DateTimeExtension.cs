using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Utilites.DatesTime
{
    public static class DateTimeExtension
    {
        //Returns the days between two takes including start date and end date
        public static IEnumerable<DateTime> GetDateRange(this DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date has to be less than end date");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

        public static string DurationInDays(this DateTime postedDate)
        {
            DateTime today = DateTime.Now;
            int daysElapsed = (today.Date - postedDate.Date).Days;

            if (daysElapsed == 0)
            {
                return "Today";
            }

            if (daysElapsed == 1)
            {
                return "1 day ago";
            }

            return String.Format("{0} days ago", daysElapsed);
        }
    }
}
