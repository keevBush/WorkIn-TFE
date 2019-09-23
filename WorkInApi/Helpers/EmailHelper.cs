using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WorkInApi.Helpers
{
    public class EmailHelper
    {
        public static void SendEmails(string subject,string body ,params string[] emails)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 25);
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential("work.in.server@gmail.com", "WorkInApi12!");
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                mail.From = new MailAddress("work.in.server@gmail.com");
                foreach(var e in emails)
                {
                    mail.To.Add(e);
                    mail.Subject = subject;
                    mail.Body = body;

                    
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
