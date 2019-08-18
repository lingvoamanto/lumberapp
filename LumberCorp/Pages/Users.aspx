﻿<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="LumberCorp.Users" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.IO" %>
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
        <table><thead>
        <tr>
            <td>First</td>
            <td>Last</td>
            <td>Email</td>
            <td>Code</td>
            <td>Approved</td>
            <td>Status</td>
            <td>Price</td>
        </tr></thead>
        <tbody>

        <% 
        string file = Server.MapPath(Path.DirectorySeparatorChar+"data") + Path.DirectorySeparatorChar + "BGA.xls";
        List<Price> prices = Price.ReadAll(file);
        int sequence = Sequence;
        int total = ContentManagementSystem.CountUsers();
        if (sequence > total - 9)
                sequence = total - 9;
        if (sequence < 1)
                sequence = 1; 
       List<User> users = ContentManagementSystem.GetUsers(sequence,20);
                
        foreach (User user in users)
        { 
            Response.Write("<tr>");
            Response.Write("<td><input type=\"text\" class=\"user-first\" id=\"first-" + user.Id + "\" value=\"" + user.First +"\" /></td>");
            Response.Write("<td><input type=\"text\" class=\"user-last\" id=\"last-" + user.Id + "\" value=\""+user.Last+"\" /></td>");
            Response.Write("<td><input type=\"text\" class=\"user-email\" id=\"email-" + user.Id + "\" value=\"" + user.Email + "\" /></td>");
            Response.Write("<td><input type=\"text\" class=\"user-code\" id=\"code-" + user.Id + "\" value=\"" + user.Code + "\" /></td>");
            Response.Write("<td><input type=\"checkbox\" id=\"approved-" + user.Id + "\" " + (user.Approved ? " checked " : "") + " /></td>");
            Response.Write("<td><select id=\"status-" + user.Id + "\">");
            Response.Write("<option value=\"0\" " + (user.Status==0?"selected=\"selected\"":"")+">Denied</option>");
            Response.Write("<option value=\"1\" " + (user.Status == 1 ? "selected=\"selected\"" : "") + ">Order</option>");
            Response.Write("<option value=\"2\" " + (user.Status == 2 ? "selected=\"selected\"" : "") + ">Administer</option>");
            Response.Write("</select></td>");
            Response.Write("<td><select id=\"price-" + user.Id + "\">");           
            foreach (Price price in prices)
            {
                Response.Write("<option value=\""+price.Name+"\" " + (user.Price == price.Name ? "selected=\"selected\"" : "") + ">"+price.Name+"</option>");
            }
            Response.Write("</select></td>");
            Response.Write("<td><button type=\"button\" class=\"delete-button\" data-id=\"" + user.Id + "\">delete</button></td>");
            Response.Write("<td><button type=\"button\" class=\"update-button\" data-id=\"" + user.Id + "\">update</button></td>");
            Response.Write("</tr>");
        } %>
        </tbody></table>
        
        <div class="paginate">
            <input id="total" type="hidden" value="<% Response.Write(total); %>" />
            <%
                    Pagination.Write(total, "/users", Sequence);
            %>
        </div>
    </div>
    

</asp:Content>
