using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Runtime.Serialization;

namespace LumberCorp
{
    [DataContract]
    public class StockItem : IComparable
    {
        [DataMember] 
        public string Category { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Grade { get; set; }
        [DataMember]
        public string Treatment { get; set; }
        [DataMember]
        public string Dryness { get; set; }
        [DataMember]
        public string Finish { get; set; }
        [DataMember]
        public string Width { get; set; }
        [DataMember]
        public string Thickness { get; set; }
        [DataMember]
        public string Length { get; set; }
        [DataMember]
        public string Packs { get; set; }
        [DataMember]
        public string Cube { get; set; }
        [DataMember]
        public string SKU { get; set; }
        [DataMember]
        public string Price { get; set; }

        public int CompareTo(object compared)
        {
            StockItem a = this;
            StockItem b = compared as StockItem;

            int compareTo = string.Compare(a.Type, b.Type);
            if (compareTo == 0)
            {
                compareTo = string.Compare(a.Category, b.Category);
                if (compareTo == 0)
                {
                    try
                    {
                        double aWidth = double.Parse(a.Width);
                        double bWidth = double.Parse(b.Width);
                        if (aWidth < bWidth)
                            compareTo = -1;
                        else if (aWidth > bWidth)
                            compareTo = 1;
                    }
                    catch
                    {
                        compareTo = string.Compare(a.Width, b.Width);
                    }

                    if( compareTo == 0 )
                    {
                        try
                        {
                            double aThickness = double.Parse(a.Thickness);
                            double bThickness = double.Parse(b.Thickness);
                            if (aThickness < bThickness)
                                compareTo = -1;
                            else if (aThickness > bThickness)
                                compareTo = 1;
                        }
                        catch
                        {
                            compareTo = string.Compare(a.Thickness, b.Thickness);
                        }

                        if( compareTo == 0 )
                        {
                            try {
                            double aLength = double.Parse(a.Length);
                            double bLength = double.Parse(b.Length);
                            if (aLength < bLength)
                                compareTo = -1;
                            else if (aLength > bLength)
                                compareTo = 1;
                            }
                            catch {
                                compareTo = string.Compare(a.Length, b.Length);
                            }

                            if( compareTo == 0 )
                            {
                                compareTo = string.Compare(a.Grade, b.Grade);
                            }
                        }
                    }
                }
            }

            return compareTo;
        }
    }

    public class StockColumn
    {
        int Category { get; set; }
        int Grade { get; set; }
        int Treatment { get; set; }
        int Dryness { get; set; }
        int Finish { get; set; }
        int Width { get; set; }
        int Thickness { get; set; }
        int Length { get; set; }
        int Packs { get; set; }
        int Cube { get; set; }
        int SKU { get; set; }
    }

    public class StockItems
    {
        public static void MethodAccessException() 
        {

        }

        public static List<StockItem> Read(string file, string priceName)
        {
            List<StockItem> stockItems = new List<StockItem>();

            // string connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=\"Excel 8.0;HDR=YES\"";
            // Create the connection object
            OleDbConnection oledbConn = new OleDbConnection(connString);



            try
            {
                // Open connection
                oledbConn.Open();

                System.Data.DataTable dbSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dbSchema == null || dbSchema.Rows.Count < 1)
                {
                    throw new Exception("Error: Could not determine the name of the first worksheet.");
                }
                string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();

                int priceColumn = 0;
                int treatmentColumn = -1;
                int gradeColumn = -1;
                int drynessColumn = -1;
                int finishColumn = -1;
                int widthColumn = -1;
                int thicknessColumn = -1;
                int lengthColumn = -1;
                int packsColumn = -1;
                int cubeColumn = -1;
                int SKUColumn = -1;
                int categoryColumn = -1;
                int typeColumn = -1;
                bool priceFound = false;



                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + firstSheetName + "]", oledbConn);

                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                OleDbDataReader reader = cmd.ExecuteReader();
                int row = 0;
                stockItems = new List<StockItem>();
                while (reader.Read())
                {
                    if (row == 0) // We need to find out which column this user's pricing information is in
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string name = reader.GetName(i);
                            if (name == "Pricing-" + priceName)
                            {
                                priceFound = true;
                                priceColumn = i;
                                break;
                            }
                            else if (name.ToLower() == "treatment")
                                treatmentColumn = i;
                            else if (name.ToLower() == "grade")
                                gradeColumn = i;
                            else if (name.ToLower() == "dryness")
                                drynessColumn = i;
                            else if (name.ToLower() == "finish")
                                finishColumn = i;
                            else if (name.ToLower() == "width")
                                widthColumn = i;
                            else if (name.ToLower() == "thickness")
                                thicknessColumn = i;
                            else if (name.ToLower() == "length")
                                lengthColumn = i;
                            else if (name.ToLower() == "packs")
                                packsColumn = i;
                            else if (name.ToLower() == "cube")
                                cubeColumn = i;
                            else if (name.ToLower() == "productcode")
                                SKUColumn = i;
                            else if (name.ToLower() == "category")
                                categoryColumn = i;
                            else if (name.ToLower() == "prodcat")
                                typeColumn = i;
                        }
                    }

                    StockItem stockItem = new StockItem();
                    string[] categoryType = reader[categoryColumn].ToString().Split('-');


                    if (categoryType.Length > 1)
                    {
                        stockItem.Category = categoryType[categoryColumn].TrimEnd();
                        stockItem.Type = categoryType[typeColumn].TrimStart();
                    }
                    else
                    {
                        stockItem.Category = categoryType[categoryColumn].TrimEnd();
                        stockItem.Type = categoryType[categoryColumn].TrimEnd();
                    }

                    if (gradeColumn != -1)
                    {
                        stockItem.Grade = reader[gradeColumn].ToString();
                        stockItem.Grade = stockItem.Grade.ToUpper();
                    }
                    if (treatmentColumn != -1)
                    {
                        stockItem.Treatment = reader[treatmentColumn].ToString();
                        stockItem.Treatment = stockItem.Treatment.ToUpper();
                    }
                    if (drynessColumn != -1)
                    {
                        stockItem.Dryness = reader[drynessColumn].ToString();
                        stockItem.Dryness = stockItem.Dryness.ToUpper();
                    }
                    if (finishColumn != -1)
                    {
                        stockItem.Finish = reader[finishColumn].ToString();
                        stockItem.Finish = stockItem.Finish.ToUpper();
                    }
                    if( widthColumn != -1 )
                        stockItem.Width = reader[widthColumn].ToString();
                    if( thicknessColumn != -1 )
                        stockItem.Thickness = reader[thicknessColumn].ToString();
                    if( lengthColumn != -1 )
                        stockItem.Length = reader[lengthColumn].ToString();
                    if( packsColumn != -1 )
                        stockItem.Packs = reader[packsColumn].ToString();
                    if( cubeColumn != -1 )
                        stockItem.Cube = reader[cubeColumn].ToString();
                    if( SKUColumn != -1 )
                        stockItem.SKU = reader[SKUColumn].ToString();
                    if (priceFound)
                        stockItem.Price = reader[priceColumn].ToString();
                    else
                        stockItem.Price = "";
                    stockItems.Add(stockItem);

                    row++;
                }

                stockItems.Sort();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                // Close connection
                oledbConn.Close();
            }

            return stockItems;
        }
    }
}