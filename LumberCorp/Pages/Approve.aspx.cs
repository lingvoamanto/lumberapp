using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class Approve : System.Web.UI.Page
    {
        List<User> users = null;
        List<Price> prices = null;
        List<Status> statuses = null;

        public List<User> UnapprovedUsers
        {
            get
            {
                if (users == null)
                {
                    users = ContentManagementSystem.GetUnapprovedUsers();
                }

                return users;
            }
        }

        public List<Status> Statuses
        {
            get
            {
                if (statuses == null)
                {
                    statuses = ContentManagementSystem.GetStatuses();
                }

                return statuses;
            }
        }

        private void Authorize()
        {
            User user = (User)HttpContext.Current.Session["user"];

            if (user == null)
                Response.Redirect("/home");

            if (user.Status != 2)
            {
                Response.Redirect("/home");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Authorize();
        }
    }
}