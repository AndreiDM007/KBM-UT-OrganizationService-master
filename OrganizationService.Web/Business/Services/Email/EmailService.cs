using System.Threading.Tasks;
using Kebormed.Core.OrganizationService.Web.Business.Services.Email.Models;
using Kebormed.Core.OrganizationService.Web.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Email
{
    // https://kenhaggerty.com/articles/article/aspnet-core-22-smtp-emailsender-implementation TODO
    public class EmailService
    {

        private readonly SmtpClient smtpClient;
        private readonly EmailSettings emailSettings;
        
        public EmailService(EmailSettings emailSettings)
        {
            smtpClient = new SmtpClient();
            this.emailSettings = emailSettings;
            smtpClient.Connect(emailSettings.MailServer, emailSettings.MailPort, true);
        }

        public Task SendEmailAsync(SendEmailCommand command)
        {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (emailSettings.SenderName, emailSettings.SenderAddress));
            message.To.Add (new MailboxAddress (command.RecipientFullName, command.RecipientAddress));
            message.Subject = command.Subject;

            message.Body = new TextPart (TextFormat.Html) {
                Text = command.Content
            };

            if (!smtpClient.IsAuthenticated)
            {
                smtpClient.Authenticate(
                    // emailSettings.SenderAddress,
                    emailSettings.SenderKey,
                    emailSettings.Password
                    );
            }

            return smtpClient.SendAsync(message);
        }
    }
}