using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace LumberCorp
{
    /// <summary>
    /// Summary description for LumberService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class LumberService : System.Web.Services.WebService
    {
        public LumberService()
        {

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string DeleteImage(string filename)
        {
            string path = "unassigned";
            try
            {
                string imageDirectory = HttpContext.Current.Server.MapPath(@"/images");
                path = Path.Combine(imageDirectory, filename);
                File.Delete(path);
                return "OK";
            }
            catch (Exception exception)
            {
                return "Can't delete " + path + " because " + exception.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Register(string email, string password, string first, string last)
        {

            User user = ContentManagementSystem.FindUser( email, password );
            if (user != null)
            {
                return "user already exists.";
            }
            else
            {
                if (ContentManagementSystem.AddUser(first, last, email, password))
                {
                    try // to subscribe them to mail chimp
                    {
                        MailChimp.ListSubscribe(email, first, last);  // try to subscribe them
                    }
                    catch
                    {
                        //
                    }

                    try  // to send out emails that they have subscribed
                    {
                        Email.TellAdministratorAboutNewUser(first, last, email);
                    }
                    catch 
                    {
                        // 
                    }

                    return "OK";
                }
                else
                    return "unable to register user.";
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string SendOrder(CartItems cartItems, string notes)
        {
            User user = (User)HttpContext.Current.Session["user"];
            if( user != null )
                return CartItems.SendOrder(cartItems.data, user, notes, "n/a");
            else 
                return "user not logged in";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string UpdateUser(string first, string last, string email, string code, bool approved, int status, string price)
        {
            User user = (User)HttpContext.Current.Session["user"];
            if (user == null)
                return "unauthorised user";
            if (user.Status == 2)
            {
                try
                {
                    ContentManagementSystem.UpdateUser(first, last, email,code, approved,status,price);
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            return "OK";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string DeleteUser(string email)
        {
            User user = (User) HttpContext.Current.Session["user"];
            if (user.Status == 2)
            {
                ContentManagementSystem.DeleteUser(email);
            }
            return "OK";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Logout()
        {
            HttpContext.Current.Session["user"] = null;
            return "OK";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Authenticate(string email, string password)
        {
            User user = ContentManagementSystem.FindUser(email, password);
            if (user != null)
            {
                if( user.Approved )
                {
                    HttpContext.Current.Session["user"] = user;
                    return "OK";
                }
                else
                {
                    HttpContext.Current.Session["authenticated"] = false;
                    return "not yet approved.";
                }
            }
            else
            {
                HttpContext.Current.Session["authenticated"] = false;
                return "unknown user.";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void SavePage(string page, List<Content> contents, List<Image> images)
        {
            Node node = ContentManagementSystem.FindNodeByPage(page);

            if (node == null)
            {
                node = new Node();
                node.Page = page;
            }

            
            for( int i=0; i<contents.Count; i++ )
            {
                contents[i].Node = node;
                Content content;
                if (i >= node.Contents.Count)
                {
                    content = contents[i];
                    content.Id = 0;
                    node.Contents.Add(contents[i]);
                }
                else
                {
                    content = node.Contents[i];
                    content.Position = contents[i].Position;
                    content.Node = node;
                    content.Html = contents[i].Html;
                    content.Id = contents[i].Id;
                }

                ContentManagementSystem.SaveContent(content);
            }

            // Remove any images that the user has deleted
            for (int i = 0; i < node.Images.Count; i++)
            {
                int id = node.Images[i].Id;
                foreach (Image image in images)
                {
                    if( image.Id == id )
                    {
                        ContentManagementSystem.RemoveImage(image);
                    }
                }
            }


            // Remove any images that the user has deleted
            foreach (Image image in images)
            {
                ContentManagementSystem.SaveImage(image);
            }

            node.Images.Clear();
            node.Images.AddRange(images);
        }
    }
}
