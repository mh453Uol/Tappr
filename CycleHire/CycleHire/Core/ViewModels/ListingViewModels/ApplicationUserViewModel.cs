using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ListingViewModels
{
    public class ApplicationUserViewModel
    {
        public Guid Id { get; set; }
        public String Firstname { get; set; }
        public String Surname { get; set; }
        public String ProfileImageId { get; set; }


        public string GetFullname()
        {
            return string.Format("{0} {1}", Firstname, Surname);
        }
    }
}
