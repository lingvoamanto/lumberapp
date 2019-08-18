using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class Register : System.Web.UI.Page
    {
        List<MenuItem> subMenu = null;
        Node currentNode = null;

        public Node CurrentNode
        {
            get
            {
                if (currentNode == null)
                {
                    string page = Page.RouteData.Values["page"] as string;
                    if (page == null)
                        page = "home";
                    currentNode = ContentManagementSystem.FindNodeByPage(page);
                }
                return currentNode;
            }
        }

        public List<MenuItem> SubMenu
        {
            get
            {
                if (subMenu == null)
                {
                    subMenu = ContentManagementSystem.FindMenuByNodeId(CurrentNode);
                }
                return subMenu;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}