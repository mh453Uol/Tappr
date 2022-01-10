using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public enum BookingStatus
    {
        PENDING,
        ACCEPTED,
        DECLINED,
        CANCELLEDBYHOST,
        CANCELLEDBYTENANT,
        NOTPOSSIBLE
    }
}
