using System;
using System.Collections.Generic;
using ExcelDataReader;
using System.IO;
using System.Runtime.Serialization;

namespace LumberCorp
{
    [DataContract]
    public class OrderItem 
    {
        DateTime required;
        [DataMember]
        public int Interval
        {
            get { return required.Millisecond; }
            set { required = new DateTime( value / 1000000); }
        }

        [DataMember]
        public bool IsDaylightSavingTime
        {
            get { return required.IsDaylightSavingTime(); }
            set { }
        }

        [DataMember]
        public string Customer { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerRef { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
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

        public DateTime Required
        {
            get;
            set;
        }
        [DataMember]
        public int Outstanding { get; set; }
        [DataMember]
        public string Notes { get; set; }
    }


    public class OrderItems : List<OrderItem>
    {
        public string cmdstring;
        public int row;
        public string message;
        public string lastActivity = "";

        public static void MethodAccessException()
        {

        }

        public static OrderItems Read(string file, User user)
        {
            return Read(file, user, true);
        }

        public static OrderItems Read(string file, User user, bool all)
        {
            OrderItems orderItems = new OrderItems();

            // null user is useless. Return nothing
            if ( user == null )
                return orderItems;

            try
            {

                orderItems.lastActivity = "string firstSheetName = dbSchema.Rows[0][\"TABLE_NAME\"].ToString();";

                int customerColumn = -1;
                int customerNameColumn = -1;
                int customerRefColumn = -1;
                int orderNoColumn = -1;
                int gradeColumn = -1;
                int treatmentColumn = -1;
                int drynessColumn = -1;
                int finishColumn = -1;
                int widthColumn = -1;
                int thicknessColumn = -1;
                int lengthColumn = -1;
                int requiredColumn = -1;
                int outstandingColumn = -1;
                int notesColumn = -1;



                // Create OleDbCommand object and select data from worksheet Sheet1
                using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        orderItems.row = 0;
                        do
                        {
                            // TODO not sure if we'll need this
                            // orderItems.lastActivity = "while (reader.Read())";

                            if (orderItems.row == 0) // We need to find out which column this user's pricing information is in
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    string name = reader.GetName(i);
                                    if (name.ToLower() == "customer")
                                        customerColumn = i;
                                    else if (name.ToLower() == "customername")
                                        customerNameColumn = i;
                                    else if (name.ToLower() == "customerref")
                                        customerRefColumn = i;
                                    else if (name.ToLower() == "orderno")
                                        orderNoColumn = i;
                                    else if (name.ToLower() == "grade")
                                        gradeColumn = i;
                                    else if (name.ToLower() == "treatment")
                                        treatmentColumn = i;
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
                                    else if (name.ToLower() == "required")
                                        requiredColumn = i;
                                    else if (name.ToLower() == "outstanding")
                                        outstandingColumn = i;
                                    else if (name.ToLower() == "notes")
                                        notesColumn = i;
                                    i++;
                                }

                                orderItems.row++;
                                continue;
                            }

                            OrderItem orderItem = new OrderItem();
                            int column = 0;
                            while (reader.Read())
                            {

                                if (column == customerColumn)
                                {
                                    orderItem.Customer = reader[customerColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Customer";
                                }
                                else if (column == customerNameColumn)
                                {
                                    orderItem.CustomerName = reader[customerNameColumn].ToString();
                                    orderItems.lastActivity = "orderItem.CustomerName";
                                }
                                else if (column == customerRefColumn)
                                {
                                    orderItem.CustomerRef = reader[customerRefColumn].ToString();
                                    orderItems.lastActivity = "orderItem.CustomerRef";
                                }
                                else if (column == orderNoColumn)
                                {
                                    orderItem.OrderNo = reader[orderNoColumn].ToString();
                                    orderItems.lastActivity = "orderItem.OrderNo";
                                }
                                else if (column == gradeColumn)
                                {
                                    orderItem.Grade = reader[gradeColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Grade";
                                }
                                else if (column == treatmentColumn)
                                {
                                    orderItem.Treatment = reader[treatmentColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Treatment";
                                }
                                else if (column == drynessColumn)
                                {
                                    orderItem.Dryness = reader[drynessColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Dryness";
                                }
                                else if (column == finishColumn)
                                {
                                    orderItem.Finish = reader[finishColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Finish";
                                }
                                else if (column == widthColumn)
                                {
                                    orderItem.Width = reader[widthColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Width";
                                }
                                else if (column == thicknessColumn)
                                {
                                    orderItem.Thickness = reader[thicknessColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Thickness";
                                }
                                else if (column == lengthColumn)
                                {
                                    orderItem.Length = reader[lengthColumn].ToString();
                                    orderItems.lastActivity = "orderItem.Length";
                                }
                                else if (column == requiredColumn)
                                {
                                    if (!reader.IsDBNull(requiredColumn))
                                    {
                                        // orderItems.lastActivity = "!reader.IsDBNull(requiredColumn) "+requiredColumn.ToString()+reader.GetFieldType(requiredColumn).ToString();
                                        DateTime required = reader.GetDateTime(requiredColumn);
                                        orderItems.lastActivity = "reader.GetDateTime(requiredColumn)";
                                        orderItem.Required = required;
                                    }
                                    orderItems.lastActivity = "requiredColumn";
                                }
                                else if (column == outstandingColumn)
                                {
                                    orderItems.lastActivity = "outstandingColumn is a " + reader.GetFieldType(outstandingColumn).ToString();
                                    orderItem.Outstanding = (Int32)reader.GetDouble(outstandingColumn);
                                    orderItems.lastActivity = "outstandingColumn";
                                }
                                else if (column == notesColumn)
                                {
                                    orderItem.Notes = reader[notesColumn].ToString();
                                    orderItems.lastActivity = "notesColumn";
                                }
                            }
                            if (all && user.Status > 1) // Admin sees everything on the web but not a mobile device
                                orderItems.Add(orderItem);
                            else if (user.Code == orderItem.Customer)
                                orderItems.Add(orderItem);

                            orderItems.row++;
                        } while (reader.NextResult());
                    }
                }
            }
            catch (Exception exception)
            {
                orderItems.message = exception.Message;
                Console.WriteLine(exception.Message);
                Email.TellWebMasterAboutError("OrderItem", exception.Message);
            }

            return orderItems;
        }
    }
}