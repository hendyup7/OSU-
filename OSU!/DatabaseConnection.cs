using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace OSU_
{
    class DatabaseConnection
    {
        public DatabaseConnection()
        {

        }

        public void UpdateDB(string dbname, string Q)
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database="+dbname+";";
            string query = Q;
            
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                databaseConnection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public List<string[]> FetchDB(string dbname, string Q)
        {
            List<string[]> result = new List<string[]>();
            int index = 0;

            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=" + dbname + ";";
            string query = Q;

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();

                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string [] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6) };
                        result.Add(row);
                        index++;

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");

                }

                databaseConnection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return result;

        }

    }
}
