using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LumberCorp
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id
        {
            get;
            set; 
        }

        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [DataMember]
        public string First
        {
            get;
            set;
        }

        [DataMember]
        public string Last
        {
            get;
            set;
        }

        [DataMember]
        public string Password
        {
            get;
            set;
        }

        [DataMember]
        public bool Approved
        {
            get;
            set;
        }

        [DataMember]
        public string Price
        {
            get;
            set;
        }

        [DataMember]
        public int Status
        {
            get;
            set;
        }

        [DataMember]
        public string Code
        // This is the TimberSmart user code, which we use to update
        {
            get;
            set;
        }
    }
}