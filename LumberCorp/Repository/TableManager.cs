using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;

namespace LumberCorp
{
    public class TableManager
    {
        internal enum Type { MySql, Sql };

        internal static Type ConnectionType
        {
            get {
                return Type.Sql;
            }
        }

        public static string ConnectionName
        {
            get
            {
                if (ConnectionType == Type.Sql)
                    return "LocalSQLServer";
                else
                    return "MySqlServer";
            }
        }

        internal static object Connection()
        {
            string connectionString = ConnectionString;
            if (ConnectionType == Type.Sql)
                return new SqlConnection(connectionString);
            else
                return new MySqlConnection(connectionString);
        }

        static internal string ConnectionString
        {
            get
            {
                return GetConnectionString(ConnectionName);
            }
        }

        internal static string GetConnectionString(string name)
        {
            // Get the connectionStrings section.
            System.Configuration.ConnectionStringsSection connectionStringsSection =
                WebConfigurationManager.GetSection("connectionStrings")
                as System.Configuration.ConnectionStringsSection;

            ConnectionStringSettingsCollection connectionStrings =
                connectionStringsSection.ConnectionStrings;


            // Get the connectionStrings key,value pairs collection.
            IEnumerator connectionStringsEnum = connectionStrings.GetEnumerator();

            int i = 0;
            while (connectionStringsEnum.MoveNext())
            {
                if (name == connectionStrings[i].Name)
                    return connectionStrings[i].ConnectionString;
                i += 1;
            }

            return "";
        }

    }


}
