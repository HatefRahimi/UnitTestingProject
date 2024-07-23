using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;

namespace HouseKeeperHelper_UnitTestingProject
{

    public class EmailSender 
    {
        public void EmailFile(string emailAddress, string emailBody, string filename, string subject)
        {
            var client = new SmtpClient(SystemSettingsHelper.EmailSmtpHost)
            {
                Port = SystemSettingsHelper.EmailPort,
                Credentials =
                    new NetworkCredential(
                        SystemSettingsHelper.EmailUsername,
                        SystemSettingsHelper.EmailPassword)
            };

            var from = new MailAddress(SystemSettingsHelper.EmailFromEmail, SystemSettingsHelper.EmailFromName,
                Encoding.UTF8);
            var to = new MailAddress(emailAddress);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8
            };

            message.Attachments.Add(new Attachment(filename));
            client.Send(message);
            message.Dispose();

            File.Delete(filename);
        }

    }
}
