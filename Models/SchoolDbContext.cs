using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace CumulativeProject.Models
{
    public class SchoolDbContext
    {
        private static string User { get { return "root"; } }

        private static string Password { get { return "root"; } }

        private static string Database { get { return "SchoolDb"; } }

        private static string Server { get { return "localhost"; } }

        private static string Port { get { return "3306"; } }

        //ConnectionString
        protected static string ConnectionString
        {
            get
            {
                //convert zero datetime is a db connection

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }

        ///<summary>
        ///Returns a connection to the blog database
        ///</summary>
        ///<example>
        ///private SchoolDbContext School = new SchoolDbContext();
        ///MySqlConnection Conn = School.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>

        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
