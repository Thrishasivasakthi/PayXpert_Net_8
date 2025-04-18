using System;
using System.IO;

namespace PayXpert_Net_8.Util
{
    public static class DBPropertyUtil
    {
        public static string GetConnectionString()
        {
            try
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconfig.txt");

                if (!File.Exists(configFilePath))
                {
                    throw new System.Exception("Database configuration file not found.");
                }

                string[] lines = File.ReadAllLines(configFilePath);
                string server = "";
                string database = "";
                string integratedSecurity = "";

                foreach (string line in lines)
                {
                    if (line.StartsWith("Server="))
                        server = line.Substring("Server=".Length);
                    else if (line.StartsWith("Database="))
                        database = line.Substring("Database=".Length);
                    else if (line.StartsWith("Integrated Security="))
                        integratedSecurity = line.Substring("Integrated Security=".Length);
                }

                if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database))
                {
                    throw new System.Exception("Invalid database configuration.");
                }

                return $"Server={server};Database={database};Integrated Security={integratedSecurity};";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Error reading database configuration: " + ex.Message);
            }
        }
    }
}