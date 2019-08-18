using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace LumberCorp
{
    public partial class Contact : TableManager
    {
        const string fields = "Name,Email,Title,Phone,Fax,Mobile,Image,Show";
        const string values = "@Name,@Email,@Title,@Phone,@Fax,@Mobile,@Image,@Show";
        const string settings = "Name=@Name,Email=@Email,Title=@Title,Phone=@Phone,Fax=@Fax,Mobile=@Mobile,Image=@Image,Show=@Show";
        const string file = "Contacts";
        const string sqldefinitions = @"
                                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [Name] [nvarchar](max) NULL,
	                                [Email] [nvarchar](max) NULL,
                                    [Phone] [nvarchar](max) NULL,
                                    [Fax] [nvarchar](max) NULL,
                                    [Mobile] [nvarchar](max) NULL,
                                    [Image] [nvarchar](max) NULL,
                                    [Title] [nvarchar](max) NULL,
                                    [Show] [int] NULL
"; const string mysqldefinitions = @"
                                    'Id' int PRIMARY KEY AUTO_INCREMEMENT NOT NULL,
	                                'Name' nvarchar(21845) NULL,
	                                'Email' nvarchar(21845) NULL,
                                    'Phone' nvarchar(21845) NULL,
                                    'Fax' nvarchar(21845) NULL,
                                    'Mobile' nvarchar(21845) NULL,
                                    'Image' nvarchar(21845) NULL,
                                    'Title' nvarchar(21845) NULL,
                                    'Show' int NULL
";

        public static List<Contact> GetAll()
        {
            List<Contact> contacts = new List<Contact>();

            if(ConnectionType == Type.Sql)
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT Id,"+fields+" FROM "+file+" ORDER BY [Id]";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        if (reader.IsDBNull(8))
                            contact.Show = false;
                        else
                            contact.Show = reader.GetInt32(8) != 0;
                        contact.Id = reader.GetInt32(0);
                        contact.Name = reader["Name"].ToString();
                        contact.Email = reader["Email"].ToString();
                        contact.Title = reader["Title"].ToString();
                        contact.Phone = reader["Phone"].ToString();
                        contact.Fax = reader["Fax"].ToString();
                        contact.Mobile = reader["Mobile"].ToString();
                        contact.Image = reader["Image"].ToString();

                        contacts.Add(contact);
                    }
                    reader.Close();
                }
            else
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT Id," + fields + " FROM " + file + " ORDER BY [Id]";
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        if (reader.IsDBNull(8))
                            contact.Show = false;
                        else
                            contact.Show = reader.GetInt32(8) != 0;
                        contact.Id = reader.GetInt32(0);
                        contact.Name = reader["Name"].ToString();
                        contact.Email = reader["Email"].ToString();
                        contact.Title = reader["Title"].ToString();
                        contact.Phone = reader["Phone"].ToString();
                        contact.Fax = reader["Fax"].ToString();
                        contact.Mobile = reader["Mobile"].ToString();
                        contact.Image = reader["Image"].ToString();

                        contacts.Add(contact);
                    }
                    reader.Close();
                }

            return contacts;
        }

        public static bool Add(string name, string email, string title, string phone, string fax, string mobile, string image, bool show)
        {
            if (ConnectionType == Type.Sql)
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO " + file + " (" + fields + ") VALUES (" + values + ")";
                // const string variables = "Name,Email,Title,Phone,Fax,Mobile";
                command.Parameters.AddWithValue("@Name", name == null ? "" : name);
                command.Parameters.AddWithValue("@Email", email == null ? "" : email);
                command.Parameters.AddWithValue("@Title", title == null ? "" : title);
                command.Parameters.AddWithValue("@Phone", phone == null ? "" : phone);
                command.Parameters.AddWithValue("@Fax", fax == null ? "" : fax);
                command.Parameters.AddWithValue("@Mobile", mobile == null ? "" : mobile);
                command.Parameters.AddWithValue("@Image", image == null ? "" : image);
                command.Parameters.AddWithValue("@Show", show ? 1 : 0);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO " + file + " (" + fields + ") VALUES (" + values + ")";
                // const string variables = "Name,Email,Title,Phone,Fax,Mobile";
                command.Parameters.AddWithValue("@Name", name == null ? "" : name);
                command.Parameters.AddWithValue("@Email", email == null ? "" : email);
                command.Parameters.AddWithValue("@Title", title == null ? "" : title);
                command.Parameters.AddWithValue("@Phone", phone == null ? "" : phone);
                command.Parameters.AddWithValue("@Fax", fax == null ? "" : fax);
                command.Parameters.AddWithValue("@Mobile", mobile == null ? "" : mobile);
                command.Parameters.AddWithValue("@Image", image == null ? "" : image);
                command.Parameters.AddWithValue("@Show", show ? 1 : 0);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public static bool Update(int id, string name, string email, string title, string phone, string fax, string mobile, string image, bool show)
        {
            if( ConnectionType == Type.Sql)
                using ( SqlConnection connection = new SqlConnection(ConnectionString ))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE "+file+" SET "+settings+" WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name == null ? "" : name);
                    command.Parameters.AddWithValue("@Email", email == null ? "" : email);
                    command.Parameters.AddWithValue("@Title", title == null ? "" : title);
                    command.Parameters.AddWithValue("@Phone", phone == null ? "" : phone);
                    command.Parameters.AddWithValue("@Fax", fax == null ? "" : fax);
                    command.Parameters.AddWithValue("@Mobile", mobile == null ? "" : mobile);
                    command.Parameters.AddWithValue("@Image", image == null ? "" : image);
                    command.Parameters.AddWithValue("@Show", show ? 1 : 0);
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch(Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        return false;
                    }
                }
            else
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE " + file + " SET " + settings + " WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name == null ? "" : name);
                    command.Parameters.AddWithValue("@Email", email == null ? "" : email);
                    command.Parameters.AddWithValue("@Title", title == null ? "" : title);
                    command.Parameters.AddWithValue("@Phone", phone == null ? "" : phone);
                    command.Parameters.AddWithValue("@Fax", fax == null ? "" : fax);
                    command.Parameters.AddWithValue("@Mobile", mobile == null ? "" : mobile);
                    command.Parameters.AddWithValue("@Image", image == null ? "" : image);
                    command.Parameters.AddWithValue("@Show", show ? 1 : 0);
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        return false;
                    }
                }
        }

        public static bool Delete(int id)
        {
            if( ConnectionType == Type.Sql)
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using( SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM " + file + " WHERE Id=Id";
                        command.Parameters.AddWithValue("@Id", id);

                        try
                        {
                            command.ExecuteNonQuery();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            else
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM " + file + " WHERE Id=Id";
                        command.Parameters.AddWithValue("@Id", id);

                        try
                        {
                            command.ExecuteNonQuery();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
        }

        public static Contact Get(int id)
        {
            Contact contact = null;


            // Return the hexadecimal string. 
            if( ConnectionType == TableManager.Type.Sql)
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT Id,"+fields+" FROM "+file+" WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contact = new Contact();
                            if (reader.IsDBNull(8))
                                contact.Show = false;
                            else
                                contact.Show = reader.GetInt32(8) != 0;
                            contact.Id = reader.GetInt32(0);
                            contact.Name = reader["Name"].ToString();
                            contact.Email = reader["Email"].ToString();
                            contact.Title = reader["Title"].ToString();
                            contact.Phone = reader["Phone"].ToString();
                            contact.Fax = reader["Fax"].ToString();
                            contact.Mobile = reader["Mobile"].ToString();
                            contact.Image = reader["Image"].ToString();
                        }
                    }
                }
            else
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT Id," + fields + " FROM " + file + " WHERE Id=@Id";
                    command.Parameters.AddWithValue("@Id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contact = new Contact();
                            if (reader.IsDBNull(8))
                                contact.Show = false;
                            else
                                contact.Show = reader.GetInt32(8) != 0;
                            contact.Id = reader.GetInt32(0);
                            contact.Name = reader["Name"].ToString();
                            contact.Email = reader["Email"].ToString();
                            contact.Title = reader["Title"].ToString();
                            contact.Phone = reader["Phone"].ToString();
                            contact.Fax = reader["Fax"].ToString();
                            contact.Mobile = reader["Mobile"].ToString();
                            contact.Image = reader["Image"].ToString();
                        }
                    }
                }

            return contact;
        }

 
        public static void CreateTable()
        {
            // connectionString = "server=localhost;Database=cherrytree;UID=root;Password=root;";
            var connection = Connection();
            {
                if( connection is SqlConnection )
                { 
                    ((SqlConnection) connection).Open();
                    // Name,Email,Title,Phone,Fax,Mobile,Image
                    SqlCommand command = ((SqlConnection)connection).CreateCommand();
                    command.CommandText = @"CREATE TABLE [dbo].[" + file + "](" + sqldefinitions + ") ";

                    try
                    { 
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        // System.Diagnostics.Debug.WriteLine(exception.Message);
                    }
                }
                else
                { 
                    ((MySqlConnection)connection).Open();
                    MySqlCommand command = ((MySqlConnection)connection).CreateCommand();
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS `" + file + "` (" + mysqldefinitions + ") ";

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
