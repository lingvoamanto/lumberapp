using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LumberCorp
{
    public class Node
    {
        List<Image> images = new List<Image>();
        List<Content> contents = new List<Content>();

        public string Title
        {
            get;
            set;
        }

        public string Page
        {
            get;
            set;
        }

        public string Custom
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public List<Content> Contents
        {
            get
            {
                return contents;
            }
        }

        public List<Image> Images
        {
            get
            {
                return images;
            }
        }
    }
}