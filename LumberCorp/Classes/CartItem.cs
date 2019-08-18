using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace LumberCorp
{
    public class CartItems
    {
        public List<CartItem> data { get; set; }
        public static string[] distribution = { "clairet@lumbercorp.co.nz" }; // , "glenh@lumbercorp.co.nz"

        public static string SendOrder(List<CartItem> data, User user, string notes, string orderNumber)
        {

            string result;

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                foreach( string address in distribution )
                	message.To.Add(address);

                if( (user.First == null || user.First == "") && (user.Last == null || user.Last == "") )
                    message.Subject = "Pre-order from " + user.Email;
                else
                    message.Subject = "Pre-order from " + user.First + " " + user.Last;
                message.From = new System.Net.Mail.MailAddress(Email.WebMaster);
                string body = "Order Number: " + orderNumber + "\n";
                int count = 0;
                foreach (CartItem cartItem in data)
                {
                    if (cartItem.sku != null)
                    {
                        count++;
                        body += cartItem.quantity + " x " + cartItem.sku;

                        body += "\n";
                    }
                }
                message.Body = body + "\n\n" + notes;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Email.Server, Email.Port);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(Email.User, Email.Password);
                smtp.Send(message);

                result = "OK";
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }

            return result;
        }
    }

    [DataContract]
    public class CartItem
    {
        [DataMember]
        public string sku { get; set; }
        [DataMember]
        public long quantity { get; set; }
    }
}