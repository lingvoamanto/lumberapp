using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LumberCorp;

namespace LumberCorp.Pages
{

    public partial class BackOrders : System.Web.UI.Page
    {
        new public LumberCorp.User User
        {
            get
            {
                return (LumberCorp.User)HttpContext.Current.Session["user"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}