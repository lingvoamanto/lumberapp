<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminControl.ascx.cs" Inherits="LumberCorp.AdminControl" %>
<%@ Import namespace="LumberCorp" %>
<% 
string type = "";
foreach (StockItem stockItem in StockItems)
{
    if (stockItem.Type != type)
    {
        Response.Write( "<a href=\"/stock/"+Stock.Encode(stockItem.Type)+"\">"+stockItem.Type+"</a><br />\n" );
        type = stockItem.Type;
    }
}
%>