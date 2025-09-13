using _95PhrEAKer.Services.IServices.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using _95PhrEAKer;
using _95PhrEAKer.Domain.EmailSetting;
using Resend;

namespace _95PhrEAKer.Services.ServicesExtension.EmailServices
{
    public class ResendSettings
    {
        public string ApiKey { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly IResend _resend;

        public EmailService(IOptions<ResendSettings> settings)
        {
            _resend = ResendClient.Create(settings.Value.ApiKey);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new EmailMessage()
            {
                From = "onboarding@resend.dev", // or your verified email
                To = toEmail,
                Subject = subject,
                HtmlBody = body
            };

            try
            {
                var response = await _resend.EmailSendAsync(email);
            }
            catch (ResendException ex)
            {
                // Log the error or rethrow with more context
                throw new Exception($"Resend API error: {ex.Message}", ex);
            }
        }
    }
    }
