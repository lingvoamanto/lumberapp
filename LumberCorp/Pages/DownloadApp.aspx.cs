using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp.Pages
{
    public partial class DownloadApp : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            String userAgent = this.Request.UserAgent;

            if ( userAgent.IndexOf("iPhone") > -1 ) 
            {

            }
            else if (userAgent.IndexOf("Android") > -1)
            {
                this.Response.Redirect("https://play.google.com/store/apps/details?id=nz.co.lumbercorp.LumberCorp");
            }
            else
            {
                this.Response.Redirect("http://www.lumbercorp.co.nz");
            }
        }
    }
}