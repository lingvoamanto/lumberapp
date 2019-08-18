using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LumberCorp
{
    public class Email
    {
        static public int Port { get { return 587; } }
        static public string Password { get { return "eEivMm44"; } } // "airport54" "eEivMm44" or "OYubvwI7"
        static public string User { get { return "mikael"; } } // 
        static public string Server { get { return "mail.orcon.net.nz"; } }
        static public string WebMaster { get { return "mikaelaldridge@gmail.com"; } } // "no-reply@lumbercorp.co.nz"

        private static string[] distribution = { "clairet@lumbercorp.co.nz", "hamish.bg@icloud.com", "glenh@lumbercorp.co.nz", "barry.LumbercorpNZ@xtra.co.nz" };
        
        public static void TellAdministratorAboutNewUser(string first, string last, string email)
        {
			
            try  // to send out emails that they have subscribed
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                foreach (string address in distribution)
                    message.To.Add(address);
                message.Subject = first + " " + last + " needs approval.";
                message.From = new System.Net.Mail.MailAddress(WebMaster);
                string body = "Click on http://www.lumbercorp.co.nz/approvals to approve new users.";
                message.Body = body;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server, Port);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Email.User, Password); 
                smtp.Send(message);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static void TellAdministratorAboutUpload(string error)
        {

            try  // to send out emails that they have subscribed
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                foreach (string address in distribution)
                    message.To.Add(address);
                message.Subject = "Error uploading from TimberSmart";
                message.From = new System.Net.Mail.MailAddress(WebMaster);
                message.Body = error;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server, Port);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Email.User, Password);
                smtp.Send(message);
            }
            catch
            {
                // 
            }
        }

        public static void TellWebMasterAboutError(string subject, string error)
        {

            try  // to send out emails that they have subscribed
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(WebMaster);
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress(WebMaster);
                message.Body = error;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server, Port);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Email.User, Password);
                smtp.Send(message);
            }
            catch
            {
                // 
            }
        }
    }
}