using System;
using System.Data.SqlClient;

namespace PayXpert_Net_8.Util
{
    public static class DBConnUtil
    {
        public static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }


        //public static int GetNextAvailableId(string tableName, SqlConnection connection)
        //{
        //    string query = $"SELECT ISNULL(MAX({tableName}ID), 0) + 1 FROM {tableName}";
        //    using (SqlCommand cmd = new SqlCommand(query, connection))
        //    {
        //        return Convert.ToInt32(cmd.ExecuteScalar());
        //    }
        //}

        public static int GetNextAvailableId(string tableName, SqlConnection connection)
        {
            string idColumn = $"{tableName}ID";
            string query = $"SELECT ISNULL(MAX({idColumn}), 0) + 1 FROM {tableName}";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result == DBNull.Value ? 0 : result);
            }
        }

        public static int GetNextAvailableIdFinancialRecord(string tableName, SqlConnection connection)
        {
            string idColumn = $"RecordID";
            string query = $"SELECT ISNULL(MAX({idColumn}), 0) + 1 FROM {tableName}";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result == DBNull.Value ? 0 : result);
            }
        }

    }
}