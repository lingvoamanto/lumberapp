<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Email.aspx.cs" Inherits="LumberCorp.Tests.Email" %>
<%@ Import namespace="LumberCorp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%             try  // to send out emails that they have subscribed
                       {
                           System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                           message.To.Add("mikael@lucis.co.nz");
                           message.Subject = "LumberCorp Email Test";
                           message.From = new System.Net.Mail.MailAddress(Email.WebMaster);
                           string body = "This is just a test that email is working.";
                           message.Body = body;
                           System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Email.Server, Email.Port);
                           smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                           smtp.Credentials = new System.Net.NetworkCredential(Email.User, Email.Password);
                           smtp.Send(message);
                       }
                       catch (Exception exception)
                       {
                           Response.Write( exception.Message + "<br />");
                       }%>
    </div>
    </form>
</body>
</html>
