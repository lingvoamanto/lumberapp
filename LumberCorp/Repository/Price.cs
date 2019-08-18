using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExcelDataReader;
using System.Configuration;
using System.IO;

namespace LumberCorp
{
    public class Price
    {
        public string Name
        {
            get;
            set;
        }

        public static List<Price> ReadAll(string file)
        {
            List<Price> prices = new List<Price>();

            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int i = 0;
                        if (reader.Read())
                        {
                            string name = reader.GetName(i);
                            if (name.Length > "Pricing-".Length && name.Substring(0, "Pricing-".Length) == "Pricing-")
                            {
                                Price price = new Price();
                                price.Name = name.Substring("Pricing-".Length, name.Length - "Pricing-".Length);
                                prices.Add(price);
                            }
                            i++;
                        }
                    }
                }
                catch (Exception debugException)
                {
                    System.Diagnostics.StackTrace stack = new System.Diagnostics.StackTrace(debugException, true);
                    string stringLineNumber = stack.GetFrame(1).GetFileLineNumber().ToString();
                    string message = "(" + stringLineNumber + ") " + debugException.Message;

                    Email.TellWebMasterAboutError("Price", message);
                    System.Diagnostics.Debug.WriteLine(message);
                }
            }
            return prices;
        }
    }
}