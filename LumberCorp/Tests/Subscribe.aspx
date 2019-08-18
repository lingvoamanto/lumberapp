<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subscribe.aspx.cs" Inherits="LumberCorp.Tests.Subscribe" %>
<%@ Import namespace="LumberCorp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%      string first = "mikael";
            string last = "aldridge";
            string email = "mikael@lucis.co.nz";
            string password = "destiny";
            User user = ContentManagementSystem.FindUser( "mikael@lucis.co.nz", "destiny" );
            if (user != null)
            {
                Response.Write( "user already exists." );
            }
            else
            {
                if (ContentManagementSystem.AddUser(first, last, email, password))
                {
                    try // to subscribe them to mail chimp
                    {
                        Response.Write("MailChimp.ListSubscribe("+email+", "+first+", "+last+");<br />");
                        MailChimp.ListSubscribe(email, first, last);  // try to subscribe them
                    }
                    catch
                    {
                        //
                    }

                    try  // to send out emails that they have subscribed
                    {
                        Response.Write("Email.TellAdministratorAboutNewUser(first, last, email);<br />");
                        Email.TellAdministratorAboutNewUser(first, last, email);
                    }
                    catch (Exception exception)
                    {
                        Response.Write(exception.Message+"<br />"); 
                    }

                    Response.Write("OK<br />");
                }
                else
                    Response.Write("unable to register user.<br />");
            }
        %>
    </div>
    </form>
</body>
</html>
