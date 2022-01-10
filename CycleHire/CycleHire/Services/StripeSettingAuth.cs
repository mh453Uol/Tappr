using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    public class StripeSettingAuth
    {
        public String SecretKey { get; set; }
        public String PublishableKey { get; set; }
        public String ClientId { get; set; }
    }
}
