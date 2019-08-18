using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class _404 : System.Web.UI.Page
    {
        Node currentNode = null;

        public Node CurrentNode
        {
            get
            {
                if (currentNode == null)
                {
                    string page = Page.RouteData.Values["page"] as string;
                    currentNode = ContentManagementSystem.FindNodeByPage("404");
                }
                return currentNode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}