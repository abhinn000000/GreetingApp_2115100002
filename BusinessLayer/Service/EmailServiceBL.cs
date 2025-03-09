using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using NLog;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{

    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool SendResetEmail(string email, string token)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("Smtp");
                string smtpHost = smtpSettings["Host"];
                int smtpPort = int.Parse(smtpSettings["Port"]);
                string smtpUsername = smtpSettings["Username"];
                string smtpPassword = smtpSettings["Password"];

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpUsername);
                    mail.To.Add(email);
                    mail.Subject = "Password Reset Request";
                    mail.Body = $"Click the link to reset your password: https://localhost:7090/reset-password?token={token}";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        smtp.EnableSsl = true;

                        _logger.LogInformation($"Attempting to send email to: {email}");
                        smtp.Send(mail);
                        _logger.LogInformation("Email sent successfully.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }

}