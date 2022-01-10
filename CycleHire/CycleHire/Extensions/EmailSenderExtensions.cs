using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CycleHire.Services;
using CycleHire.Core.ViewModels;
using CycleHire.Core.ViewModels.EmailTemplatesViewModels;
using CycleHire.Core.Models;

namespace CycleHire.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string subject, string firstname, string link, string templateFile)
        {
            return emailSender.SendEmailAsync(email, subject, "", templateFile,
                new ConfirmationEmailModel(link, firstname));
        }

        public static Task SendNewBookingEmailAsync(this IEmailSender emailSender, string email, string bookingLink, string imageLink, Booking booking)
        {
            return emailSender.SendEmailAsync(email, "New Booking", "", "PendingBooking", new PendingBookingEmailModel()
            {
                From = booking.From,
                To = booking.To,
                HostEarning = booking.SubTotal,
                BookingLink = bookingLink,
                ListingImage = imageLink,
                ListingTitle = booking.Listing.Title,
                TenantName = booking.User.FullName()
            });
        }

    }
}
