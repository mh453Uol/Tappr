using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }


        public static string NewBookingCallbackLink(this IUrlHelper urlHelper, Guid id, string scheme)
        {
            return urlHelper.Action(
                action: nameof(HostController.Index),
                controller: "Host",
                values: null,
                protocol: scheme);
        }
    }
}
