using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LumberCorp
{
    public class MenuItem
    {
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

        public string Title
        {
            get;
            set;
        }

        public Node Node
        {
            get;
            set;
        }

        public string Page
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }
    }
}