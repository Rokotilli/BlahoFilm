using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Enums;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BusinessLogicLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string content, SendEmailActions action)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("BlahoFilm", _config["EmailConfirmation:EmailAddress"]));
                mimeMessage.To.Add(new MailboxAddress(email, email));
                mimeMessage.Subject = "Confirmation email";
                var builder = new BodyBuilder();

                if (action == SendEmailActions.ConfirmEmail)
                {
                    builder.HtmlBody = @"<p>Press button to confirm email</p>
                     <a href='" + content + @"' style='background-color: #4CAF50; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;'>Confirm</a>";
                }
                if (action == SendEmailActions.ChangePassword)
                {
                    builder.HtmlBody = @"<p>Press button to start changing your password</p>
                     <a href='" + content + @"' style='background-color: #4CAF50; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;'>Start</a>";
                }                

                mimeMessage.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_config["EmailConfirmation:SMTPServerHost"], int.Parse(_config["EmailConfirmation:SMTPServerPort"] ?? "0"), false);
                    smtp.Authenticate(_config["EmailConfirmation:EmailAddress"], _config["EmailConfirmation:Password"]);
                    smtp.Send(mimeMessage);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Sending email failed!");
            }
        }
    }
}
