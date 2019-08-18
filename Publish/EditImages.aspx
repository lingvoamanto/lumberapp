<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="EditImages.aspx.cs" Inherits="LumberCorp.EditImages" %>

<%@ Register TagPrefix="uc" TagName="AdminControl" Src="~/AdminControl.ascx" %>
<%@ Import namespace="System.Data.OleDb" %>
<%@ Import namespace="System.Configuration" %>
<%@ Import namespace="System.IO" %>
<%@ Import namespace="LumberCorp" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            Response.Write("<img src=\"/images/IMG_1099-FRONT3.png\" alt=\"\" class=\"active\" />");
        %>
    </div>
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 
        <uc:AdminControl id="adminControl" runat="server" />
    </div>
    <div class="content">
        <table><thead>
        <tr>
            <td>Image</td>
            <td>File</td>
            <td>Action</td>
        </tr></thead>
        <tbody>

        <% 
            string directory = HttpContext.Current.Server.MapPath(@"/images");
            string [] fileEntries = Directory.GetFiles( directory );

            foreach (string path in fileEntries)
            {
                string fileName = Path.GetFileName(path);
                Response.Write("<tr>");
                Response.Write("<td><img src=\"/images/"+fileName+"\" width=\"200\" /></td>");
                Response.Write("<td><p>"+fileName+"</p></td>");
                Response.Write("<td><button type=\"button\" class=\"delete-image-button\" data-file=\"" + fileName + "\">delete</button></td>");
                Response.Write("</tr>");
            } %>
        </tbody></table>
    </div>
</asp:Content>

<asp:Content runat="server" ID="ScriptContent" ContentPlaceHolderID="ScriptContent">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.delete-image-button').click(function () {
                var filename = $(this).data('file');

                $.ajax({
                    type: "POST",
                    contentType: 'application/json',
                    url: "/LumberService.asmx/DeleteImage",
                    dataType: 'json',
                    data: JSON.stringify({
                        filename: filename
                    }),
                    success: function (result) {
                        location.reload();
                    },
                    error: function (jqXHR, status, error) {
                        alert("Unable to delete image. Please, try later.");
                    }
                });
            });
        });
    </script>
</asp:Content>
