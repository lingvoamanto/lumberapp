using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using LumberCorp;
// using System.Web.Script.Serialization;
                            
//using System.Runtime.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace LumberCorp.Handlers
{
    /// <summary>
    /// Summary description for MobileHandler
    /// </summary>
    /// 
   [DataContract]
    public class MobileRequest
    {
        [DataMember]
        public LumberCorp.User User { get; set; }

        [DataMember]
        public string Command { get; set; }
    }

    [DataContract]
    public class MobileResponse
    {
        [DataMember]
        public LumberCorp.User User { get; set; }
        [DataMember]
        public List<StockItem> StockItems { get; set; }
        [DataMember]
        public OrderItems BackItems { get; set; }
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public LayoutMenu Menu { get; set; }
        [DataMember]
        public List<Contact> Contacts { get; set; }
    }

    [DataContract]
    public class LayoutMenu
    {
        [DataMember]
        public List<Type> Types { get; set; }
    }

    [DataContract]
    public class Type
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Id { get; set; }
    }

    public class MobileHandler : IHttpHandler
    {
        // Use a POST to handle the incoming data
        public void ProcessRequest(HttpContext context)
        {
            // 
            StreamWriter writer = new StreamWriter(context.Response.OutputStream);
            MobileResponse mobileResponse = new MobileResponse();
            MobileRequest mobileRequest=null;

            try
            {
                byte[] key = Convert.FromBase64String("Wmf84Y1KsN1tKvxoGohBMLm/qLzq1HpU0/nsmi215xc=");
                byte[] IV = Convert.FromBase64String("JI24AwSI+EyB/LR2d+1+Lw==");

                // encrypt it
                StreamReader reader = new StreamReader(context.Request.InputStream);
                // string encoded = reader.ReadToEnd();
                // reader.Close();
                // byte[] encrypted = Convert.FromBase64String(encoded);
                // byte[] decrypted = AES.DecryptBytesFromBytes(encrypted, key, IV);

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MobileRequest));
                // MemoryStream requestStream = new MemoryStream(decrypted);
                // requestStream.Position = 0;

                mobileRequest = (MobileRequest)serializer.ReadObject(context.Request.InputStream);
                if (mobileRequest == null)
                {
                    mobileResponse.Error = "Request is null";
                }
                else
                {
                    User user = ContentManagementSystem.FindUser(mobileRequest.User.Email, mobileRequest.User.Password);

                    if (user == null  && mobileRequest.Command == "Register" )
                    {
                        if (ContentManagementSystem.AddUser("", "", mobileRequest.User.Email, mobileRequest.User.Password))
                        {
                            mobileResponse.User = ContentManagementSystem.FindUser(mobileRequest.User.Email, mobileRequest.User.Password);
                            Email.TellAdministratorAboutNewUser("","",mobileRequest.User.Email);
                            MailChimp.ListSubscribe(mobileRequest.User.Email, "", "");
                        }
                        else
                        {
                            mobileResponse.Error = "Bad password";
                        }
                    }
                    else if (user != null)
                    {
                        if (user.Password != mobileRequest.User.Password)
                            mobileResponse.Error = "Bad password";
                        else
                        {
                            mobileResponse.User = user;
                            mobileResponse.Contacts = Contact.GetAll();
                            string file = context.Server.MapPath("/data") + "\\BGA.xls";
                            string backFile = context.Server.MapPath("/data") + "\\Orders.xls";
                            mobileResponse.StockItems = StockItems.Read(file, mobileResponse.User.Price);
                            mobileResponse.BackItems = OrderItems.Read(backFile, mobileResponse.User, false);

                            string menustring = @"{""Types"": [
 {""Name"": ""Construction Timber"", ""Id"": ""Construction"", ""Categories"" :
     [
        {""Name"": ""Structural Framing"", ""Id"": ""Structural Framing""},
        {""Name"": ""Strapping"", ""Id"": ""Strapping""},
        {""Name"": ""Cavity Batten"", ""Id"": ""Cavity Batten""}
     ]
 },
 {""Name"": ""Outdoor Timber"", ""Id"": ""Outdoor"", ""Categories"":
     [
      {""Id"": ""Decking/ PremiumBalustrades"", ""Name"": ""Decking (Merchant Grade;Premium)""},
      {""Id"": ""Fencing"", ""Name"": ""Fencing (Palings; Rails; Posts)""},
      {""Id"": ""Landscaping"", ""Name"": ""Landscaping""},
      {""Id"": ""Merch/Sawn/D4S"", ""Name"": ""Merchant Sawn D4S""},
      {""Id"": ""Stuctural"", ""Name"": ""Structural""}
     ]
 },
 {""Name"": ""Industrial/Boxing"", ""Id"": ""Industrial/ Boxing/ Misc"", ""Categories"":
     [
      {""Name"": ""Industrial/Boxing"", ""Id"": ""Industrial/ Boxing/ Misc""}
     ]
 }
]}";
                            MemoryStream menuStream = new MemoryStream();
                            byte[] bytes = new byte[menustring.Length * sizeof(char)];
                            System.Buffer.BlockCopy(menustring.ToCharArray(), 0, bytes, 0, bytes.Length);
                            menuStream.Write(bytes, 0, bytes.Length);
                            menuStream.Seek(0, SeekOrigin.Begin);
                            DataContractJsonSerializer menuSerializer = new DataContractJsonSerializer(typeof(LayoutMenu));
                            mobileResponse.Menu = (LayoutMenu)menuSerializer.ReadObject(menuStream);
                        }
                    }
                    else if (user == null)
                    {
                        mobileResponse.Error = "Unknown user";
                    }
                    else
                    {
                        mobileResponse.Error = "Invalid command";
                    }
                }
            }
            catch (Exception e)
            {
                mobileResponse = new MobileResponse() { Error = e.Message };
            }
            finally
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MobileResponse));
                serializer.WriteObject(context.Response.OutputStream, mobileResponse);
                writer.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}