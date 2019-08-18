<%@ Control  Debug="true" Language="C#" AutoEventWireup="true" CodeBehind="AdminControl.ascx.cs" Inherits="LumberCorp.AdminControl" %>
<%@ Import namespace="LumberCorp"%>

<% 
    User user = (User) HttpContext.Current.Session["user"];

    if (user != null)
    {
        if (user.Status == 2)
        {
            Response.Write("<div class=\"module\">");
            Response.Write("<h3>User Management</h3>");
            Response.Write("<a href=\"/users\">Update users</a><br />");
            Response.Write("<a href=\"/approvals\">Approve new users</a><br />");
            Response.Write("</div>");

            Response.Write("<div class=\"module\">");
            Response.Write("<h3>Content Management</h3>");
            Response.Write("<a href=\"/edit/images\">Images</a><br />");
            Response.Write("</div>");

            System.Collections.Generic.List<Node> nodes = ContentManagementSystem.GetNodesIgnoringContentAndImages();
            if( nodes != null )
                if (nodes.Count > 0)
                {
                    Response.Write("<div class=\"module\">");
                    Response.Write("<h3>Page Management</h3>");
                    foreach (Node node in nodes)
                    {
                        Response.Write("<a href=\"/edit/" + node.Page + "\">" + node.Title + "</a><br />\n");
                        
                    }
                    Response.Write("</div>");
                }
        }
    }
    
    if ((Page.RouteData.Values["page"] as string) == "sales")
    {
%>
        <div class="module" id="login" <%= (user == null ? "" : "style=\"display:none\"") %>>
            <h3>Please enter your details to login.</h3>
            <p id="login-text"></p>
            <label for="login-email">Email</label>
            <input type="email" id="login-email" size="20"/><br />
            <label for="login-password">Password</label>
            <input type="password" id="login-password" size="20"/><br />
            <button type="button" id="login-button">Login</button><br />
            <button type="button" id="go-register-button">Register</button>
        </div>
        
        <div id="logout" <%= (user != null ? "" : "style=\"display:none\"") %>>
            <button type="button" id="logout-button">Logout</button>
        </div>
<%
    }
            
    

%>