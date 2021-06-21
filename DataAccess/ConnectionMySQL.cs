using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace DataAccess
{
    public abstract class ConnectionMySQL
    {
        private readonly string connectionstring;
     


        public ConnectionMySQL(){
            connectionstring = @"Server=localhost;Database=wabutt;Uid=root;";
        }

        protected MySqlConnection GetConnection() {
                return new MySqlConnection(connectionstring);
        }


    }
}
