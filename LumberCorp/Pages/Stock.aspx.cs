using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;

namespace LumberCorp
{
    public partial class Stock : Page
    {
        List<StockItem> stockItems;

        static public string Encode(string value)
        {
            if (value == null)
                return "";
            string result = value.Replace("/", "_");
            return result;
        }

        static public string Decode(string value)
        {
            if (value == null)
                return "";
            string result = value.Replace("_", "/");
            return result;
        }

        public List<StockItem> StockItems
        {
            get { return stockItems; }
        }

        public string Category
        {
            get
            {
                return Decode(Page.RouteData.Values["category"] as string);
            }
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
            User user = (User)HttpContext.Current.Session["user"];
            string file = Server.MapPath(System.IO.Path.DirectorySeparatorChar+"data") + System.IO.Path.DirectorySeparatorChar + "BGA.xls";
            stockItems = LumberCorp.StockItems.Read(file,user == null ? "" : user.Price);
            stockControl.StockItems = stockItems;
        } 
    }
}