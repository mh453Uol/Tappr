using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels
{
    public class NoItemsViewModel
    {
        public NoItemsViewModel(string heading, string description, string icon)
        {
            Heading = heading;
            Description = description;
            Icon = icon;
        }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
