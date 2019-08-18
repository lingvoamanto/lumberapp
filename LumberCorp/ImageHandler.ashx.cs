using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace LumberCorp
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public static void WriteFileFromStream(Stream stream, string toFile)
        {
            using (FileStream fileToSave = new FileStream(toFile, FileMode.OpenOrCreate))
            {
                stream.CopyTo(fileToSave);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            // 
            string feedback = "we have nothing to say";

            try
            {
                // encrypt it
                StreamReader reader = new StreamReader(context.Request.InputStream);
                string uploadDir = context.Server.MapPath(@"/images");
                HttpPostedFile file = context.Request.Files[0];
                WriteFileFromStream(file.InputStream,Path.Combine(uploadDir, file.FileName));

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