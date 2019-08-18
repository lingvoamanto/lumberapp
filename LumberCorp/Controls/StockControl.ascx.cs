using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class StockControl : System.Web.UI.UserControl
    {
        public List<StockItem> StockItems {get;set;}

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}