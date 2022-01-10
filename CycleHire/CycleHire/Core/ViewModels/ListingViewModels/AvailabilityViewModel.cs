using CycleHire.Utilites.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ListingViewModels
{
    public class AvailabilityViewModel
    {
        [Display(Name = "Availability (Days)")]
        public List<string> DayOfWeek { get; set; }

        [MinimumItem(1, ErrorMessage = "Please select as least one day of the week")]
        public List<string> SelectedDayOfWeek { get; set; }

        public AvailabilityViewModel()
        {
            PopulateDaysOfWeek();
            SelectedDayOfWeek = new List<string>();
        }

        public void PopulateDaysOfWeek()
        {
            DayOfWeek = new List<string>()
            {
                "Monday","Tuesday","Wenesday",
                "Thursday","Friday","Saturday",
                "Sunday"
            };
        }
    }
}
