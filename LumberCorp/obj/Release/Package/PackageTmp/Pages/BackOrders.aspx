<%@ Page Title="Back Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackOrders.aspx.cs" Inherits="LumberCorp.Pages.BackOrders" %>

<%@ Import namespace="System.Data.OleDb" %>
<%@ Import namespace="System.Configuration" %>
<%@ Import namespace="LumberCorp" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            Response.Write( "<img src=\"/images/front-1.png\" alt=\"\" class=\"active\" />" );
        %>
    </div>
    <hr style="clear:both">
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 

    </div>
    <div class="content">

        <% 
        string file = Server.MapPath("/data") + "\\Orders.xls";
        User user =  (User)HttpContext.Current.Session["user"];
        OrderItems orderItems = OrderItems.Read(file,user);

        int status = (user == null ? 0 : user.Status);
        string userCustomer = (user == null ? "" : user.Code);
        string currentCustomer = "";
        bool first = true;

        if (user == null)
        {
            %><p>You do not have permission to look at this page.</p> <% 
        }
        else if (orderItems.Count == 0)
        {
            %><p>You have nothing on backorder.</p> <% 
        }
        else
        {
            foreach (OrderItem orderItem in orderItems)
            {

                        
                if (orderItem.Customer == userCustomer || status>1)
                {
                    if (currentCustomer != orderItem.Customer)
                    {
                        currentCustomer = orderItem.Customer;
                        if (!first)
                        {
                            Response.Write("</tbody></table><br />\n");
                        }
                        else
                        {
                            first = false;
                        }
                        Response.Write("<span class=\"order-customer\">" + orderItem.Customer + "</span><br />\n");
                        Response.Write("<span class=\"order-customername\">" + orderItem.CustomerName + "</span><br />\n");
                        %>
                                <table class="order">
                                <thead>
                                    <tr>
                                        <td>Ref</td>
                                        <td>Order No</td>
                                        <td>Grade</td>
                                        <td>Treatment</td>
                                        <td>Dryness</td>
                                        <td>Finish</td>
                                        <td>Width</td>
                                        <td>Thickness</td>
                                        <td>Length</td>
                                        <td>Required</td>
                                        <td>Outstanding</td>
                                        <td>Notes</td>
                                    </tr>
                                </thead>
                                <tbody>
                            <%
                    }
                }
                




                Response.Write("<tr>");
                Response.Write("<td class=\"order-customerref\"><div>" + orderItem.CustomerRef + "</div></td>");
                Response.Write("<td class=\"order-orderno\"><div>" + orderItem.OrderNo + "</div></td>");
                Response.Write("<td class=\"order-grade\"><div>" + orderItem.Grade + "</div></td>");
                Response.Write("<td class=\"order-treatment\"><div>" + orderItem.Treatment + "</div></td>");
                Response.Write("<td class=\"order-dryness\"><div>" + orderItem.Dryness + "</div></td>");
                Response.Write("<td class=\"order-finish\"><div>" + orderItem.Finish + "</div></td>");
                Response.Write("<td class=\"order-width\"><div>" + orderItem.Width + "</div></td>");
                Response.Write("<td class=\"order-thickness\"><div>" + orderItem.Thickness + "</div></td>");
                Response.Write("<td class=\"order-length\"><div>" + orderItem.Length + "</div></td>");
                Response.Write("<td class=\"order-required\"><div>" + orderItem.Required.ToString("d/MMM/yy") + "</div></td>");
                Response.Write("<td class=\"order-outstanding\"><div>" + orderItem.Outstanding + "</div></td>");
                Response.Write("<td class=\"order-notes\"><div>" + orderItem.Notes + "</div></td>");
                Response.Write("</tr>");
            } 
        }
        %>
        </tbody></table>
    </div>
</asp:Content>

