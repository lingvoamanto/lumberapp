using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace StockUploader
{


    class Program
    {
        const string handler = @"http://www.lumbercorp.co.nz/LumberHandler.ashx";
        // const string handler = @"http://localhost:52217/LumberHandler.ashx";

        static void Main(string[] args)
        {
            byte[] key = Convert.FromBase64String("Wmf84Y1KsN1tKvxoGohBMLm/qLzq1HpU0/nsmi215xc=");
            byte[] IV = Convert.FromBase64String("JI24AwSI+EyB/LR2d+1+Lw==");

            string source = args.Length > 0 ? args[0] : "C:\\Users\\Boyd Price\\Dropbox\\LumberCorp\\Publish\\data\\BGA.xls";

            System.IO.FileStream fileStream = new FileStream(source, FileMode.Open);

            // read the xls file in
            byte[] file = ReadFully(fileStream); // 
            // encrypt it
            byte[] encrypt = AES.EncryptBytesToBytes(file, key, IV);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(handler);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            

            string encoded = Convert.ToBase64String(encrypt);
            request.ContentLength = encoded.Length;

            using (Stream stream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(encoded);
                }
            }

            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                   Console.Write(reader.ReadToEnd());
                }
            }
        }


        public static byte[] ReadFully(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
