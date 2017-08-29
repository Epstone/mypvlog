namespace PVLog.Controllers
{
    using System.Configuration;
    using System.Net.Mail;
    using Utility;

    public interface IEmailSender
    {
        void Send(string body, string subject, string recipientAddress);
    }

    internal class EmailSender : IEmailSender
    {
        public void Send(string body, string subject, string recipientAddress)
        {
            try
            {
                string server = ConfigurationManager.AppSettings["SmtpServer"];
                string password = ConfigurationManager.AppSettings["SmtpPassword"];
                string user = ConfigurationManager.AppSettings["SmtpUser"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

                using (var smtpClient = new SmtpClient(server, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(user, password);

                    smtpClient.Send("info@mypvlog.de",
                        recipientAddress,
                        subject,
                        body);
                }
                Logger.LogInfo($"Sent email successfully to {recipientAddress}.");
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex, SeverityLevel.Warning, "Could not send email.");
            }
        }
    }
}