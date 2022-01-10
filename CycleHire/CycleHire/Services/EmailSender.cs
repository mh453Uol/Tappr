using FluentEmail.Core;
using FluentEmail.Mailgun;
using FluentEmail.Razor;
using FluentEmail.SendGrid;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSettingAuth Options { get; }
        public EmailSender(IOptions<EmailSettingAuth> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message, string templateFile, object templateData)
        {
            var sender = new SendGridSender(Options.ApiKey);

            Email.DefaultSender = sender;
            Email.DefaultRenderer = new RazorRenderer();


            var emailSending = Email
                .From(Options.From)
                .To(email)
                .Subject(subject)
                .Body(message);

            if (!String.IsNullOrEmpty(templateFile))
            {
                var assembly = this.GetType().GetTypeInfo().Assembly;
                var targets = assembly.GetManifestResourceNames();

                var templatefile = targets.First(file => file.Contains(templateFile));

                emailSending.UsingTemplateFromEmbedded(templatefile, templateData, this.GetType().GetTypeInfo().Assembly);
            }

            await emailSending.SendAsync();
        }
    }
}
