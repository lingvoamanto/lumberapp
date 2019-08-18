<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/Pages/EditContacts.aspx.cs" Inherits="LumberCorp.Pages.Contacts" %>
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
        <table>
        <tbody>

        <% 
        string file = Server.MapPath("/data") + "\\BGA.xls";
        List<Contact> contacts = Contact.GetAll();
        foreach (Contact contact in contacts)
        { 
            Response.Write("<tr>");
            Response.Write("<td><table><tr><td><img src='/images/profiles/"+contact.Image+"' /></td></tr>");
            Response.Write("<tr><td><input type=\"text\" id=\"image-" + contact.Id + "\" value=\"" + contact.Image + "\" /></td></tr></table></td>");
            Response.Write("<td><table>");
            Response.Write("<tr><td>Name</td><td><input type=\"text\" id=\"name-" + contact.Id + "\" value=\"" + contact.Name +"\" /></td></tr>");
            Response.Write("<tr><td>Title</td><td><input type=\"text\" id=\"title-" + contact.Id + "\" value=\"" + contact.Title + "\" /></td>");
            Response.Write("<tr><td>Email</td><td><input type=\"text\" id=\"email-" + contact.Id + "\" value=\"" + contact.Email + "\" /></td>");
            Response.Write("<tr><td>Phone</td><td><input type=\"text\" id=\"phone-" + contact.Id + "\" value=\"" + contact.Phone + "\" /></td>");
            Response.Write("<tr><td>Fax</td><td><input type=\"text\" id=\"fax-" + contact.Id + "\" value=\"" + contact.Fax + "\" /></td>");
            Response.Write("<tr><td>Mobile</td><td><input type=\"text\" id=\"mobile-" + contact.Id + "\" value=\"" + contact.Mobile + "\" /></td>");
            Response.Write("<tr><td>Show</td><td><input type=\"checkbox\" id=\"show-" + contact.Id + "\" "+(contact.Show?"checked=\"checked\"":"")+" /></td>");
            Response.Write("</table></td>");
            Response.Write("<td><table>");
            Response.Write("<tr><td><button type=\"button\" class=\"delete-contact-button\" data-id=\"" + contact.Id + "\">delete</button></td></tr>");
            Response.Write("<tr><td><button type=\"button\" class=\"update-contact-button\" data-id=\"" + contact.Id + "\">update</button></td></tr>");
            Response.Write("</table></td>");
            Response.Write("</tr>");
        } %>
        </tbody></table>
        <br />
        <button type="button" id="insert-contact-button" >add</button>
    </div>
</asp:Content>
