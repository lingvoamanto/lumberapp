using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LumberCorp
{
    [DataContract]
    public class MobileResponse
    {
        [DataMember] LumberCorp.User User { get; set; }
        [DataMember] List<StockItem> StockItems {get;set;}
        [DataMember] OrderItems BackItems {get; set;}
    }
}