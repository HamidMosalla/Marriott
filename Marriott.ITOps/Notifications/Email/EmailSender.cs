using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Marriott.ITOps.Notifications.Email
{
    public class EmailSender : ISendEmail
    {
        public async Task SendMail(Email email)
        {
            var message = new MailMessage(email.From, email.To)
            {
                Body = email.Body,
                IsBodyHtml = email.BodyIsHtml,
                Subject = email.Subject
            };

            if (!string.IsNullOrWhiteSpace(email.Cc))
                message.CC.Add(email.Cc);

            if (!string.IsNullOrWhiteSpace(email.Bcc))
                message.Bcc.Add(email.Bcc);

            if (email.Attachment != null)
                message.Attachments.Add(new Attachment(new MemoryStream(email.Attachment), email.AttachmentName, email.AttachmentMimeType));

            await SendMessage(message);
        }

        protected virtual Task SendMessage(MailMessage message)
        {
            var smtpClient = new SmtpClient { Host = ConfigurationManager.AppSettings["SmtpHost"], Credentials = CredentialCache.DefaultNetworkCredentials };
            smtpClient.SendAsync(message, null);
            return Task.CompletedTask;
        }
    }
}
