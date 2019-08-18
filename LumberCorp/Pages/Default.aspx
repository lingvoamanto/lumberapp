<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/Pages/Default.aspx.cs" Inherits="LumberCorp._Default" %>
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
                    Response.Write("<img src=\"" + image.Url + "\" alt=\"\"" + (first ? " class=\"active\" " : "") + " />");
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
           }
%>

        <% if ((Page.RouteData.Values["page"] as string) == "sales") 
        { %>
            <uc:StockControl id="stockControl" runat="server" />
        <% } %>
        <uc:AdminControl id="adminControl1" runat="server" />
                    

    </div>
    <div class="content">
        <% if( CurrentNode != null )
               Response.Write(CurrentNode.Contents[0].Html); %>
    </div>
</asp:Content>