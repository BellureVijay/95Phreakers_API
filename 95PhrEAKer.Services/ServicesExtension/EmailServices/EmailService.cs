using _95PhrEAKer.Services.IServices.EmailServices;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace _95PhrEAKer.Services.ServicesExtension.EmailServices
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly SendGridSettings _settings;

        public EmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new ArgumentException("SendGrid API key is not configured.", nameof(settings));

            _client = new SendGridClient(_settings.ApiKey);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail)) throw new ArgumentException("toEmail is required", nameof(toEmail));

            var from = new EmailAddress(string.IsNullOrWhiteSpace(_settings.FromEmail) ? "no-reply@example.com" : _settings.FromEmail,
                                        string.IsNullOrWhiteSpace(_settings.FromName) ? "NoReply" : _settings.FromName);

            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: body);

            var response = await _client.SendEmailAsync(msg);

            if (response == null)
            {
                throw new Exception("SendGrid: no response received when sending email.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid API error: {response.StatusCode} - {responseBody}");
            }
        }
    }
}
