using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace LumberCorp.Services
{
    /// <summary>
    /// Summary description for ContactService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   [System.Web.Script.Services.ScriptService]
    public class ContactService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string Update(int id, string name, string email, string title, string phone, string fax, string mobile, string image, bool show)
        {
            User user = (User)HttpContext.Current.Session["user"];
            if (user == null)
                return "unauthorised user";
            if (user.Status == 2)
            {
                try
                {
                    Contact.Update(id, name, email, title, phone, fax, mobile, image,show);
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
        public string Add()
        {
            User user = (User)HttpContext.Current.Session["user"];
            if (user == null)
                return "unauthorised user";
            if (user.Status == 2)
            {
                try
                {
                    Contact.Add("","","","","","","",false);
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
        public string Delete(int id)
        {
            User user = (User)HttpContext.Current.Session["user"];
            if (user == null)
                return "unauthorised user";
            if (user.Status == 2)
            {
                try
                {
                    Contact.Delete(id);
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            return "OK";
        }
    }
}
