using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public enum UserType
    {
        [Display(Name = "Rent someone elses cycle out")]
        Tenant = 0,

        [Display(Name = "Rent my cycle out to others")]
        Host = 1

    }
}
