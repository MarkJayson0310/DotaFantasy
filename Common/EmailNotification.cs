using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;

namespace Common
{
    public class EmailNotification
    {

        public void NotifyNewUsserForActivation(string recipient, string activationCode)
        {
            string emailBody = EmailTemplates.NewUserWithActivation.Replace("[activationCode]", activationCode).Replace("[emailAdd]", recipient);
            SendMail(emailBody, recipient, "[Eskrima] Activate your new account");
        }

        public bool SendMail(string emailBody, string sendTo, string subject)
        {
            string smtpHost = ConfigurationManager.AppSettings["SMTPHost"];
            var message = new MailMessage();
            message.To.Add(sendTo);
            message.From = new MailAddress("eskrima747@gmail.com");
            message.Subject = subject;
            message.Body = emailBody;
            message.IsBodyHtml = true;
            
            try
            {
                var smtp = new SmtpClient(smtpHost);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("eskrima747@gmail.com", "eskrimamarch101984"); 
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
               
                smtp.Send(message);

                return true;
                ///return "pass";
            }
            catch (Exception e)
            {
                return false;
                //string s = e.Message.ToString();// + "- inner exception" + e.InnerException.InnerException.ToString() + "inner exception message" + e.InnerException.Message.ToString();
                //string replacement = Regex.Replace(s, @"\t|\n|\r", "");
                //return replacement;
            }
        }
    }
}
