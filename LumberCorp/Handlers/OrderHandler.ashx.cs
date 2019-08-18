using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace LumberCorp.Handlers
{
    /// <summary>
    /// Summary description for OrderHandler
    /// </summary>
    public class OrderHandler : IHttpHandler
    {
        [DataContract]
        class Cart
        {
            [DataMember]
            public List<LumberCorp.CartItem> Items {get;set;}

            [DataMember]
            public User User {get; set;}

            [DataMember]
            public string Notes { get; set; }

            [DataMember]
            public string OrderNumber { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {          
            string result = "Nothing was done.";
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Cart));
                Cart cart = (Cart)serializer.ReadObject(context.Request.InputStream);
                result = CartItems.SendOrder(cart.Items, cart.User, cart.Notes, cart.OrderNumber);
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }
            finally
            {
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write(result);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}