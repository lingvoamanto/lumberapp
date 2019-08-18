using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class SiteMaster : MasterPage
    {
        Node currentNode;
        List<MenuItem> mainMenu;

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

        public List<MenuItem> MainMenu
        {
            get
            {
                if (mainMenu == null)
                {
                    mainMenu = ContentManagementSystem.FindMenuByNodeId(null);
                }
                return mainMenu;
            }
        }
    }
}