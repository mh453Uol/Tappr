using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.EmailTemplatesViewModels
{
    public class ConfirmationEmailModel
    {
        public ConfirmationEmailModel(string confirmLink, string firstname)
        {
            FirstName = firstname;
            ConfirmLink = confirmLink;
        }

        public string ConfirmLink { get; set; }
        public string FirstName { get; set; }
    }
}
