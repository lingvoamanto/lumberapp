using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace LumberCorp
{
    public partial class Cart : Page
    {
        List<StockItem> stockItems;

        public List<StockItem> StockItems
        {
            get { return stockItems; }
        }

        static public string Decode(string value)
        {
            if (value == null)
                return "";
            string result = value.Replace("_", "/");
            return result;
        }

        public string Type
        {
            get
            {
                return Decode(Page.RouteData.Values["type"] as string);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // string connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
            string file = Context.Server.MapPath("/data") + "\\BGA.xls";
            User user = (User)HttpContext.Current.Session["user"];
            stockItems = LumberCorp.StockItems.Read(file, user == null ? "Default Cube" : user.Price);

            stockControl.StockItems = stockItems;
        }
    }
}