<%@ Page Debug="true" Title="Approve" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" Inherits="LumberCorp.Approve" %>
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
            if (UnapprovedUsers == null)
                Response.Write("<p>There are no new users to approve</p>");
            else if( UnapprovedUsers.Count == 0  )
                Response.Write("<p>There are no new users to approve</p>");
            else
            { %>
            <table>
                <thead>
                    <tr>
                        <td>First</td>
                        <td>Last</td>
                        <td>Email</td>
                        <td>Approved</td>
                        <td>Status</td>
                    </tr>
                </thead>
                <tbody>
            <%
                foreach (User user in UnapprovedUsers)
                { 
                    Response.Write("<tr>");
                    Response.Write("<td><input type=\"text\" id=\"first-" + user.Id + "\" value=\"" + user.First +"\" /></td>");
                    Response.Write("<td><input type=\"text\" id=\"last-" + user.Id + "\" value=\""+user.Last+"\" /></td>");
                    Response.Write("<td><input type=\"text\" id=\"email-" + user.Id + "\" value=\"" + user.Email + "\" /></td>");
                    Response.Write("<td><input type=\"checkbox\" id=\"approved-" + user.Id + "\" " + (user.Approved ? " checked " : "") + " /></td>");
                    Response.Write("<td><select id=\"status-" + user.Id + "\">");
                    Response.Write("<option value=\"0\" " + (user.Status==0?"selected=\"selected\"":"")+">Denied</option>");
                    Response.Write("<option value=\"1\" " + (user.Status == 1 ? "selected=\"selected\"" : "") + ">Order</option>");
                    Response.Write("<option value=\"2\" " + (user.Status == 2 ? "selected=\"selected\"" : "") + ">Administer</option>");
                    Response.Write("</select></td>");
                    Response.Write("<td><button type=\"button\" class=\"delete-button\" data-id=\"" + user.Id + "\">delete</button></td>");
                    Response.Write("<td><button type=\"button\" class=\"update-button\" data-id=\"" + user.Id + "\">update</button></td>");
                    Response.Write("</tr>");
                } 
                Response.Write("</tbody></table>");
            }
           %>
    </div>
</asp:Content>
