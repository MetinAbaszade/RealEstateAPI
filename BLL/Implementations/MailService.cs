using System.Net.Mail;
using System.Net;
using CORE.Config;
using CORE.Abstract;

namespace BLL.Implementations;
internal class MailService(ConfigSettings config) : IMailService
{
    public async Task SendMailAsync(string email, string message)
    {
        if (!string.IsNullOrEmpty(email) && email.Contains('@'))
        {
            var fromAddress = new MailAddress(config.MailSettings.Address, config.MailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);

            var smtp = new SmtpClient
            {
                Host = config.MailSettings.Host,
                Port = int.Parse(config.MailSettings.Port),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, config.MailSettings.MailKey)
            };

            using var data = new MailMessage(fromAddress, toAddress)
            {
                Subject = config.MailSettings.Subject,
                Body = message
            };

            await smtp.SendMailAsync(data);
        }
    }
}
