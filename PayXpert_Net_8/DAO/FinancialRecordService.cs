
using PayXpert_Net_8.DAO.Interfaces;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using PayXpert_Net_8.Exception;
using PayXpert_Net_8.Util;
using PayXpert_Net_8.DAO.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PayXpert_Net_8.DAO.Services
{
    public class FinancialRecordService : IFinancialRecordService
    {
        private readonly string connectionString;

        public FinancialRecordService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType)
        {
            try
            {
                ValidationService.ValidateFinancialRecord(employeeId, description, amount, recordType);

                // Check if employee exists (if employeeId is provided)
                if (employeeId > 0)
                {
                    EmployeeService employeeService = new EmployeeService();
                    employeeService.GetEmployeeById(employeeId);
                }

                //FinancialRecord record = new FinancialRecord
                //{   
                //    EmployeeID = employeeId,
                //    RecordDate = DateTime.Now,
                //    Description = description,
                //    Amount = amount,
                //    RecordType = recordType
                //};

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    connection.Open();
                    FinancialRecord record = new FinancialRecord
                    {
                        RecordID = DBConnUtil.GetNextAvailableIdFinancialRecord("FinancialRecord", connection),
                        EmployeeID = employeeId,
                        RecordDate = DateTime.Now,
                        Description = description,
                        Amount = amount,
                        RecordType = recordType
                    };
                    //record.RecordID = DBConnUtil.GetNextAvailableId("FinancialRecord", connection);
                    string query = @"INSERT INTO FinancialRecord (RecordID, EmployeeID, RecordDate, Description, Amount, RecordType)
                                   VALUES (@RecordID, @EmployeeID, @RecordDate, @Description, @Amount, @RecordType)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RecordID", record.RecordID);
                        command.Parameters.AddWithValue("@EmployeeID",
                            record.EmployeeID > 0 ? (object)record.EmployeeID : DBNull.Value);
                        command.Parameters.AddWithValue("@RecordDate", record.RecordDate);
                        command.Parameters.AddWithValue("@Description", record.Description);
                        command.Parameters.AddWithValue("@Amount", record.Amount);
                        command.Parameters.AddWithValue("@RecordType", record.RecordType);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new FinancialRecordException("Error adding financial record: " + ex.Message);
            }
        }

        public FinancialRecord GetFinancialRecordById(int recordId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM FinancialRecord WHERE RecordID = @RecordID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RecordID", recordId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new FinancialRecord
                                {
                                    RecordID = Convert.ToInt32(reader["RecordID"]),
                                    EmployeeID = reader["EmployeeID"] != DBNull.Value ?
                                        Convert.ToInt32(reader["EmployeeID"]) : 0,
                                    RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                                    Description = reader["Description"].ToString(),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    RecordType = reader["RecordType"].ToString()
                                };
                            }
                        }
                    }
                }
                throw new FinancialRecordException($"Financial record with ID {recordId} not found.");
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving financial record: " + ex.Message);
            }
        }

        public List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId)
        {
            List<FinancialRecord> records = new List<FinancialRecord>();

            try
            {
                // Check if employee exists
                EmployeeService employeeService = new EmployeeService();
                employeeService.GetEmployeeById(employeeId);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM FinancialRecord WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new FinancialRecord
                                {
                                    RecordID = Convert.ToInt32(reader["RecordID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                                    Description = reader["Description"].ToString(),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    RecordType = reader["RecordType"].ToString()
                                });
                            }
                        }
                    }
                }
                return records;
            }
            catch (EmployeeNotFoundException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving financial records: " + ex.Message);
            }
        }

        public List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate)
        {
            List<FinancialRecord> records = new List<FinancialRecord>();

            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = @"SELECT * FROM FinancialRecord 
                                   WHERE CONVERT(date, RecordDate) = CONVERT(date, @RecordDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RecordDate", recordDate);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new FinancialRecord
                                {
                                    RecordID = Convert.ToInt32(reader["RecordID"]),
                                    EmployeeID = reader["EmployeeID"] != DBNull.Value ?
                                        Convert.ToInt32(reader["EmployeeID"]) : 0,
                                    RecordDate = Convert.ToDateTime(reader["RecordDate"]),
                                    Description = reader["Description"].ToString(),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    RecordType = reader["RecordType"].ToString()
                                });
                            }
                        }
                    }
                }
                return records;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving financial records: " + ex.Message);
            }
        }
    }
}