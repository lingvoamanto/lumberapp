using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace LumberCorp
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public StockItem Item { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        public void SendCart(User user, List<Order> orders)
        {

        }
    }
}