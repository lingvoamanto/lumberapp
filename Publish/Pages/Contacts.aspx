<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/Pages/Contacts.aspx.cs" Inherits="LumberCorp.Pages.Contacts" %>
<%@ Import namespace="System.Data.OleDb" %>
<%@ Import namespace="System.Configuration" %>
<%@ Import namespace="LumberCorp" %>

<%@ Register TagPrefix="uc" TagName="StockControl" Src="~/Controls/StockControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="AdminControl" Src="~/Controls/AdminControl.ascx" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            bool first = true;
            if (CurrentNode != null)
            {
                foreach (LumberCorp.Image image in CurrentNode.Images)
                {
                    Response.Write("<img src=\"" + image.Url + "\" alt=\"\"" + (first ? " class=\"active\">" : "") + " />");
                    first = false;
                }
            }
        %>
    </div>
    <hr style="clear:both">
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 
        <% if (CurrentNode != null)
           {
               if (CurrentNode.Contents.Count >= 2)
                   Response.Write(CurrentNode.Contents[1].Html);
           }%>


        <% if ((Page.RouteData.Values["page"] as string) == "sales") 
        { %>
            <uc:StockControl id="stockControl" runat="server" />
        <% } %>
        <uc:AdminControl id="adminControl1" runat="server" />
                    

    </div>
    <div class="content">
        <table id="contact-table" style="height: 154px; width:353px">
        <tbody>

        <% 
        string file = Server.MapPath("/data") + "\\BGA.xls";
        List<Contact> contacts = Contact.GetAll();
        foreach (Contact contact in contacts)
        { 
            Response.Write("<tr>");
            Response.Write("<td><img src=\"/images/profiles/"+contact.Image+"\" alt=\"\" /></td>");
            Response.Write("<td>"+contact.Name);
            Response.Write("<br />"+contact.Title);
            Response.Write("<br />Email: <a href=\"mailto:"+contact.Email+"\">"+contact.Email+"</a>");
            Response.Write("<br /> PH:"+contact.Phone);
            if( contact.Fax != null && contact.Fax != "" )
                Response.Write("<br /> Fax:"+contact.Fax);
            Response.Write("<br /> Mob:"+contact.Mobile);
            Response.Write("</td></tr>");
        } %>
        </tbody></table>
    </div>
</asp:Content>
