<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LumberCorp.Register" %>


<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            bool first = true;
            foreach (LumberCorp.Image image in CurrentNode.Images)
            {
                Response.Write( "<img src=\""+image.Url+"\" alt=\"\"" +  ( first ? " class=\"active\">" : "" ) + " />" );
                first = false;
            }
        %>
    </div>
    <hr style="clear:both">
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 
        <% if( CurrentNode.Contents.Count >= 2 )
               Response.Write(CurrentNode.Contents[1].Html); %>

    </div>
    <div class="content">
        <div id="register-input">
            <label for="register-first" style="width:120px;display: inline-block;">First Name</label><input id="register-first" type="text" /><br />
            <label for="register-last" style="width:120px;display: inline-block;">Last Name</label><input id="register-last" type="text" /><br />
            <label for="register-email" style="width:120px;display: inline-block;">Email</label><input id="register-email" type="text" /><br />
            <label for="register-password" style="width:120px;display: inline-block;">Password</label><input id="register-password" type="password" /><br />
            <label for="register-password2" style="width:120px;display: inline-block;">Retype password</label><input id="register-password2" type="password" /><br />
            <button type="button" id="register-button">Register</button>
        </div>
        <p id="register-text"></p>
    </div>
</asp:Content>

