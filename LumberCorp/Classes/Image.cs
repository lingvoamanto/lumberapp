using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LumberCorp
{
    public class Image
    {
        public string Url
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public Node Node
        {
            get;
            set;
        }
    }
}