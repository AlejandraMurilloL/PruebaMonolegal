using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Datos.MailServices
{
    public class MasterMailServer
    {
        private SmtpClient smtpClient;
        private string senderMail = "emailpruebasp@gmail.com";
        private string password = "pruebas de programacion";
        private string host  = "smtp.gmail.com";
        private int port = 587;
        private bool ssl = true;

        public void initializeSmtpClient()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(senderMail, password);
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
        }

        public void SendMail(string subject, string body, string recipientEmail)
        {
            initializeSmtpClient();
            var mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress(senderMail);
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.Priority = MailPriority.Normal;
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
            }finally
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }
    }
}
