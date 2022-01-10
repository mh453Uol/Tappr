using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.HomeViewModels
{
    public class SearchResultViewModel
    {
        public SearchViewModel Searched { get; set; }
        public IEnumerable<Listing> Listing { get; set; }
    }
}
