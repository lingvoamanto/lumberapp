using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExcelDataReader;
using System.IO;
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

            string error = "";

            try
            {

                int priceColumn = -1;
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



                using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                {

                    error = "reader";

                    stockItems = new List<StockItem>();
                    error = "read";

                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Just read the first sheet, as there should be only one
                        int row = 0;
                        while (reader.Read())
                        {
                            if (row == 0) // We need to find out which column this user's pricing information is in
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string name = reader.GetString(i);
                                    System.Diagnostics.Debug.WriteLine(name);
                                    if (name.Length > 8 && name.Substring(0, 8).ToLower() == "pricing-")
                                    {
                                        string priceNameSubstring = name.Substring(8).Trim();
                                        if (priceNameSubstring == priceName.Trim())
                                        {
                                            priceColumn = i;
                                            break;
                                        }
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
                            else
                            {
                                StockItem stockItem = new StockItem();

                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    stockItem.Price = "";

                                    if (column == categoryColumn)
                                    {
                                        string[] categoryType = reader.GetString(column).Split('-');

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
                                    }

                                    if (column == gradeColumn)
                                    {
                                        stockItem.Grade = reader.GetString(gradeColumn);
                                        stockItem.Grade = stockItem.Grade.ToUpper();
                                    }
                                    else if (column == treatmentColumn)
                                    {
                                        stockItem.Treatment = reader.GetString(treatmentColumn);
                                        stockItem.Treatment = stockItem.Treatment.ToUpper();
                                    }
                                    else if (column == drynessColumn)
                                    {
                                        stockItem.Dryness = reader.GetString(drynessColumn);
                                        stockItem.Dryness = stockItem.Dryness.ToUpper();
                                    }
                                    else if (column == finishColumn)
                                    {
                                        stockItem.Finish = reader.GetString(finishColumn);
                                        stockItem.Finish = stockItem.Finish.ToUpper();
                                    }
                                    else if (column == widthColumn)
                                        stockItem.Width = reader.GetString(widthColumn);
                                    else if (column == thicknessColumn)
                                        stockItem.Thickness = reader[thicknessColumn].ToString();
                                    else if (column == lengthColumn)
                                        stockItem.Length = reader.GetString(lengthColumn);
                                    else if (column == packsColumn)
                                    {
                                        stockItem.Packs = reader[packsColumn].ToString();
                                    }
                                    else if (column == cubeColumn)
                                        stockItem.Cube = reader.GetString(cubeColumn);
                                    else if (column == SKUColumn)
                                        stockItem.SKU = reader.GetString(SKUColumn);
                                    else if (column == priceColumn)
                                        stockItem.Price = reader.GetString(priceColumn);
                                    stockItems.Add(stockItem);

                                    column++;
                                }
                            }
                            row++;
                        } 
                    }
                }

                stockItems.Sort();
            }
            catch (Exception debugException)
            {
                System.Diagnostics.StackTrace stack = new System.Diagnostics.StackTrace(debugException, true);
                string stringLineNumber = stack.GetFrame(1).GetFileLineNumber().ToString();
                string message = "(" + stringLineNumber + ") " + debugException.Message;

                Email.TellWebMasterAboutError("StockItem", error + "\n" + message);
                System.Diagnostics.Debug.WriteLine(error);
                System.Diagnostics.Debug.WriteLine(message);
            }

            return stockItems;
        }
    }
}