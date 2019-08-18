using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace LumberCorp
{
    public class ContentManagementSystem : TableManager
    {
        public static List<Status> GetStatuses()
        {
            List<Status> statuses = new List<Status>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id,Name FROM Statuses ORDER BY Id";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Status status = new Status();
                status.Id = int.Parse(reader["Id"].ToString());
                status.Name = reader["Name"].ToString();
                statuses.Add(status);
            }
            reader.Close();
            connection.Close();

            return statuses;
        }

        public static List<User> GetUnapprovedUsers()
        {
            List<User> users = new List<User>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id,First,Last,Email,Code, Approved,Price,Status FROM Users WHERE [Approved] IS NULL OR [Approved]=0 ORDER BY [Email]";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["Id"].ToString());
                user.First = reader["First"].ToString();
                user.Last = reader["Last"].ToString();
                user.Email = reader["Email"].ToString();
                user.Code = reader["Code"].ToString();
                string approved = reader["Approved"].ToString();
                if (approved == "" || approved == null)
                    user.Approved = false;
                else
                    user.Approved = int.Parse(approved) != 0;
                user.Price = reader["Price"].ToString();
                string status = reader["Status"].ToString();
                if (status == "" || status == null)
                    user.Status = 1;
                else
                    user.Status = int.Parse(status);

                users.Add(user);
            }
            reader.Close();
            connection.Close();

            return users;
        }


        public static int CountUsers()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Users";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }

            return count;
        }

        public static List<User> GetUsers(int sequence, int count)
        {
            List<User> users = new List<User>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM (SELECT Id,First,Last,Email,Code,Approved,Price,Status, ROW_NUMBER() OVER(ORDER BY [Email] Asc) AS RN FROM Users ) a WHERE RN >= " + sequence + " AND RN < " + (sequence + count).ToString();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["Id"].ToString());
                user.First = reader["First"].ToString();
                user.Last = reader["Last"].ToString();
                user.Email = reader["Email"].ToString();
                user.Code = reader["Code"].ToString();
                string approved = reader["Approved"].ToString();
                if (approved == "" || approved == null)
                    user.Approved = false;
                else
                    user.Approved = int.Parse(approved) != 0;
                user.Price = reader["Price"].ToString();
                string status = reader["Status"].ToString();
                if (status == "" || status == null)
                    user.Status = 1;
                else
                    user.Status = int.Parse(status);

                users.Add(user);
            }
            reader.Close();
            connection.Close();

            return users;
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id,First,Last,Email,Code,Approved,Price,Status, Password FROM Users ORDER BY [Email]";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["Id"].ToString());
                user.First = reader["First"].ToString();
                user.Last = reader["Last"].ToString();
                user.Email = reader["Email"].ToString();
                user.Code = reader["Code"].ToString();
                user.Password = reader["Password"].ToString();
                string approved = reader["Approved"].ToString();
                if( approved == "" || approved == null )
                    user.Approved = false;
                else
                    user.Approved = int.Parse(approved) != 0;
                user.Price = reader["Price"].ToString();
                string status = reader["Status"].ToString();
                if (status == "" || status == null)
                    user.Status = 1;
                else
                    user.Status = int.Parse(status);

                users.Add(user);
            }
            reader.Close();
            connection.Close();

            return users;
        }

        public static bool AddUser(string first, string last, string email, string password)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (First,Last,Email,Password) VALUES (@First,@Last,@Email,@Password)";
            command.Parameters.AddWithValue("@First", (first == null ? "" : first));
            command.Parameters.AddWithValue("@Last", (last == null ? "" : last));
            command.Parameters.AddWithValue("@Email", (email == null ? "" : email));
            command.Parameters.AddWithValue("@Password", (password == null ? "" : password));

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

        public static bool UpdateUser(string first, string last, string email, string code, bool approved, int status, string price)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Users SET First=@First,Last=@Last,Approved=@Approved,Status=@Status,Price=@Price,Code=@Code WHERE Email=@Email";
            command.Parameters.AddWithValue("@First", (first == null ? "" : first));
            command.Parameters.AddWithValue("@Last", (last == null ? "" : last));
            command.Parameters.AddWithValue("@Email", (email == null ? "" : email));
            command.Parameters.AddWithValue("@Code", (code == null ? "" : code));
            command.Parameters.AddWithValue("@Approved", (approved? 1 : 0));
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@Price", price);
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

        public static bool DeleteUser(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Users WHERE Email=@Email";
            command.Parameters.AddWithValue("@Email", (email == null ? "" : email));

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

        public static User FindUser(string email,string password)
        {
            User user = null;

            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            string hash = stringBuilder.ToString();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id,First,Last,Approved,Price,Status,Password, Code FROM Users WHERE [Email] = @Email";
            command.Parameters.AddWithValue("@Email", (email == null ? "" : email));
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = new User();
                user.Email = email;
                user.Id = int.Parse(reader["Id"].ToString());
                user.First = reader["First"].ToString();
                user.Last = reader["Last"].ToString();
                user.Code = reader["Code"].ToString();
                string approved = reader["Approved"].ToString();
                if (approved != "")
                    user.Approved = int.Parse(approved) != 0;
                else
                    user.Approved = false;
                user.Price = reader["Price"].ToString();
                string status = reader["Status"].ToString();
                if (status == "")
                    user.Status = 1;
                else
                    user.Status = int.Parse( status );
                user.Password = reader["Password"].ToString();
            }
            reader.Close();
            connection.Close();

            return user;
        }

        public static void SaveContent(Content content)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            if (content.Id == 0)
            {
                command.CommandText = "INSERT INTO [Contents] (Node,Position,Html) VALUES (@Node,@Position,@Html)";
            }
            else
            {
                command.CommandText = "UPDATE [Contents] Set Node=@Node, Position=@Position, Html=@Html WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", content.Id);
            }
            command.Parameters.AddWithValue("@Node", content.Node.Id);
            command.Parameters.AddWithValue("@Position", content.Position);
            command.Parameters.AddWithValue("@Html", content.Html);
            command.ExecuteNonQuery();

            if (content.Id == 0)
            {
                command = new SqlCommand("SELECT @@Identity AS [ContentId]", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    content.Id = int.Parse(reader["ContentId"].ToString());
                else
                    return;
                reader.Close();
            }

            connection.Close();
        }

        public static void SaveImage(Image image)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            if (image.Id == 0)
            {
                command.CommandText = "INSERT INTO [Images] (Node,Priority,Url) VALUES (@Node,@Priority,@Url)";
                command.Parameters.AddWithValue("@Node", image.Node.Id);
                command.Parameters.AddWithValue("@Priority", image.Priority);
                command.Parameters.AddWithValue("@Url", image.Url);
            }
            else
            {
                command.CommandText = "UPDATE [Images] Set Node=@Node, Priority=@Priority, Url=@Url WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", image.Id);
                command.Parameters.AddWithValue("@Node", image.Node.Id);
                command.Parameters.AddWithValue("@Priority", image.Priority);
                command.Parameters.AddWithValue("@Url", image.Url);
            }
            command.ExecuteNonQuery();

            if (image.Id == 0)
            {
                command = new SqlCommand("SELECT @@Identity AS [ImageId]", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    image.Id = int.Parse(reader["ImageId"].ToString());
                else
                    return;
                reader.Close();
            }

            connection.Close();
        }

        public static void RemoveImage(Image image)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();

            command.CommandText = "DELETE FROM [Images] WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", image.Id);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public static List<MenuItem> FindMenuByNodeId(Node node)
        {
            List<MenuItem> menu = new List<MenuItem>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id,Priority,Title,Page,Status FROM MenuItems WHERE [Node] = @Node ORDER BY [Priority]";
            command.Parameters.AddWithValue("@Node", (node == null ? 0 : node.Id));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Id = int.Parse(reader["Id"].ToString());
                menuItem.Priority = int.Parse(reader["Priority"].ToString());
                menuItem.Title = reader["Title"].ToString();
                menuItem.Node = node;
                menuItem.Page = reader["Page"].ToString();
                if (reader.IsDBNull(4))
                    menuItem.Status = 0;
                else
                    menuItem.Status = reader.GetInt32(4);

                menu.Add(menuItem);
            }
            reader.Close();
            connection.Close();

            return menu;
        }
         
        public static Node FindNodeByPage(string page)
        {
            Node node = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Nodes WHERE [Page] = @Page";
            command.Parameters.AddWithValue("@Page", page);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                node = new Node();
                node.Id = int.Parse(reader["Id"].ToString());
                node.Custom = reader["Custom"].ToString();
                node.Title = reader["Title"].ToString();
                node.Page = page;
            }
            reader.Close();
            if (node == null)
            {
                connection.Close();
                return null;
            }

            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contents WHERE [Node] = @Node ORDER BY [Position]";
            command.Parameters.AddWithValue("@Node", node.Id);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Content content = new Content();
                content.Id = int.Parse(reader["Id"].ToString());
                content.Html = reader["Html"].ToString();
                content.Position = int.Parse(reader["Position"].ToString());
                content.Node = node;
                node.Contents.Add(content);
            }
            reader.Close();

            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Images WHERE [Node] = @Node ORDER BY [Priority]";
            command.Parameters.AddWithValue("@Node", node.Id);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Image image = new Image();
                image.Id = int.Parse(reader["Id"].ToString());
                image.Url = reader["Url"].ToString();
                image.Priority = int.Parse(reader["Priority"].ToString());
                image.Node = node;
                node.Images.Add(image);
            }
            reader.Close();

            connection.Close();
            return node;
        }

        public static List<Node> GetNodesIgnoringContentAndImages()
        {
            List<Node> nodes = new List<Node>();

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Nodes ORDER BY [Title]";

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Node node = new Node();
                node.Id = int.Parse(reader["Id"].ToString());
                node.Custom = reader["Custom"].ToString();
                node.Title = reader["Title"].ToString();
                node.Page = reader["Page"].ToString();

                nodes.Add(node);
            }
            reader.Close();
            connection.Close();

            return nodes;
        }
    }
}