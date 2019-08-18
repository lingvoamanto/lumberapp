using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class Users : System.Web.UI.Page
    {
        List<User> users = null;
        List<Price> prices = null;
        List<Status> statuses = null;

        int sequence;

        public int Sequence
        {
            get
            {
                if (sequence == 0)
                {
                    string sequenceString = Page.RouteData.Values["sequence"] as string;
                    if (sequenceString != null)
                    {
                        try
                        {
                            sequence = int.Parse(sequenceString);
                        }
                        catch { }
                    }
                }
                return sequence;
            }
        }


        public List<User> AllUsers
        {
            get
            {
                if (users == null)
                {
                    users = ContentManagementSystem.GetAllUsers();
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
            // Authorize();
        }
    }
}