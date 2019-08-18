<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="StockControl.ascx.cs" Inherits="LumberCorp.StockControl" %>
<%@ Import namespace="LumberCorp" %>
<% 
string type = "";

User user = (User) HttpContext.Current.Session["user"];
if ( user != null ) // only viewable if a user is actually logged in
{
    if (StockItems != null)
    {
        Response.Write("<div class=\"module\">");
        Response.Write("<h3>Timber types</h3>");
        foreach (StockItem stockItem in StockItems)
        {
            if (stockItem.Type != type)
            {
                Response.Write("<a href=\"/stock/" + Stock.Encode(stockItem.Type) + "\">" + stockItem.Type + "</a><br />\n");
                type = stockItem.Type;
            }
        }
        Response.Write("</div>");
    }
}
%>