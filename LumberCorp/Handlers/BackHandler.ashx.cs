using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace LumberCorp.Handlers
{
    /// <summary>
    /// Summary description for BackHandler
    /// </summary>
    public class BackHandler : IHttpHandler
    {

        // Use a POST to handle the incoming data
        public void ProcessRequest(HttpContext context)
        {
            // 
            string feedback = "Something went wrong but we don't know what.";

            try
            {
                byte[] key = Convert.FromBase64String("Wmf84Y1KsN1tKvxoGohBMLm/qLzq1HpU0/nsmi215xc=");
                byte[] IV = Convert.FromBase64String("JI24AwSI+EyB/LR2d+1+Lw==");

                // encrypt it
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string encoded = reader.ReadToEnd();
                reader.Close();
                byte[] encrypted = Convert.FromBase64String(encoded);
                byte[] decrypted = AES.DecryptBytesFromBytes(encrypted, key, IV);

                System.IO.FileStream fileStream = new FileStream(context.Server.MapPath(@"/data/Orders.xls"), FileMode.OpenOrCreate);
                BinaryWriter writer = new BinaryWriter(fileStream);
                writer.Write(decrypted);
                writer.Close();

                feedback = "success";
            }
            catch (Exception e)
            {
                feedback = e.Message;
            }
            finally
            {
                StreamWriter writer = new StreamWriter(context.Response.OutputStream);
                writer.Write(feedback);
                writer.Close();
            }

            if (feedback != "success")
            {
                Email.TellAdministratorAboutUpload(feedback);
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