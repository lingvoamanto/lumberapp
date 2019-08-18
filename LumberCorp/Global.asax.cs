using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using LumberCorp;

namespace LumberCorp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Initialise database
            Contact.CreateTable();
            // Code that runs on application startup
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void RegisterRoutes(RouteCollection routes)
        {
            // routes.MapPageRoute("", "home", "Default.aspx?id=1");
            routes.MapPageRoute("", "app", "~/Pages/DownloadApp.aspx");
            routes.MapPageRoute("", "register", "~/Pages/Register.aspx");
            routes.MapPageRoute("", "edit/contact", "~/Pages/EditContacts.aspx");
            routes.MapPageRoute("", "contact", "~/Pages/Contacts.aspx");
            routes.MapPageRoute("", "approvals", "~/Pages/Approve.aspx");
            routes.MapPageRoute("", "cart", "~/Pages/Cart.aspx");
            routes.MapPageRoute("", "stock/{type}/{category}", "~/Pages/Stock.aspx");
            routes.MapPageRoute("", "stock/{type}", "~/Pages/Stock.aspx");
            routes.MapPageRoute("", "users/{sequence}", "~/Pages/Users.aspx");
            routes.MapPageRoute("", "users", "~/Pages/Users.aspx");
            routes.MapPageRoute("", "authenticate", "~/Authenticate.aspx");
            routes.MapPageRoute("", "edit/images", "~/EditImages.aspx");
            routes.MapPageRoute("", "edit/{page}", "~/Edit.aspx");
            routes.MapPageRoute("", "home", "~/Pages/Default.aspx");
            routes.MapPageRoute("", "backorders", "~/Pages/BackOrders.aspx");
            routes.MapPageRoute("", "{page}", "~/Pages/Default.aspx");
            routes.MapPageRoute("", "", "~/Pages/Default.aspx");
        }
    }
}
