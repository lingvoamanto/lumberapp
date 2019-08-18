using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LumberCorp
{
    [DataContract]
    public partial class Contact
    {
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Title
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
        public string Phone
        {
            get;
            set;
        }

        [DataMember]
        public string Fax
        {
            get;
            set;
        }

        [DataMember]
        public string Mobile
        {
            get;
            set;
        }

        [DataMember]
        public string Image
        {
            get;
            set;
        }

        [DataMember]
        public bool Show
        {
            get;
            set;
        }
    }
}