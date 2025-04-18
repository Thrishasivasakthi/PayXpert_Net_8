using PayXpert_Net_8.DAO.Interfaces;
using PayXpert_Net_8.Entity;
using PayXpert_Net_8.Exception;
using PayXpert_Net_8.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PayXpert_Net_8.DAO.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string connectionString;

        public EmployeeService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public void AddEmployee(Employee employeeData)
        {
            try
            {
                ValidationService.ValidateEmployee(employeeData);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString)) // Updated to use Microsoft.Data.SqlClient.SqlConnection
                {
                    connection.Open();
                    employeeData.EmployeeID = DBConnUtil.GetNextAvailableId("Employee", connection);
                    string query = @"INSERT INTO Employee (EmployeeID,FirstName, LastName, DateOfBirth, Gender, Email, 
                                       PhoneNumber, Address, Position, JoiningDate, TerminationDate)
                                       VALUES (@EmployeeID, @FirstName, @LastName, @DateOfBirth, @Gender, @Email, 
                                       @PhoneNumber, @Address, @Position, @JoiningDate, @TerminationDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeData.EmployeeID);
                        command.Parameters.AddWithValue("@FirstName", employeeData.FirstName);
                        command.Parameters.AddWithValue("@LastName", employeeData.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", employeeData.DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", employeeData.Gender);
                        command.Parameters.AddWithValue("@Email", employeeData.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", employeeData.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", employeeData.Address);
                        command.Parameters.AddWithValue("@Position", employeeData.Position);
                        command.Parameters.AddWithValue("@JoiningDate", employeeData.JoiningDate);
                        command.Parameters.AddWithValue("@TerminationDate",
                            employeeData.TerminationDate.HasValue ? (object)employeeData.TerminationDate.Value : DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidInputException("Error adding employee: " + ex.Message);
            }
        }

        public Employee GetEmployeeById(int employeeId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString)) // Updated to use Microsoft.Data.SqlClient.SqlConnection
                {
                    string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Employee
                                {
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Gender = reader["Gender"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Position = reader["Position"].ToString(),
                                    JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                                    TerminationDate = reader["TerminationDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["TerminationDate"]) : (DateTime?)null
                                };
                            }
                        }
                    }
                }
                throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
            }
            catch (EmployeeNotFoundException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving employee: " + ex.Message);
            }
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString)) // Updated to use Microsoft.Data.SqlClient.SqlConnection
                {
                    string query = "SELECT * FROM Employee";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new Employee
                                {
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                    Gender = reader["Gender"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Position = reader["Position"].ToString(),
                                    JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                                    TerminationDate = reader["TerminationDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["TerminationDate"]) : (DateTime?)null
                                });
                            }
                        }
                    }
                }
                return employees;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving employees: " + ex.Message);
            }
        }

        public void RemoveEmployee(int employeeId)
        {
            try
            {
                // Check if employee exists
                GetEmployeeById(employeeId);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString)) // Updated to use Microsoft.Data.SqlClient.SqlConnection
                {
                    string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (EmployeeNotFoundException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error removing employee: " + ex.Message);
            }
        }

        public void UpdateEmployee(Employee employeeData)
        {
            try
            {
                ValidationService.ValidateEmployee(employeeData);

                // Check if employee exists
                GetEmployeeById(employeeData.EmployeeID);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString)) // Updated to use Microsoft.Data.SqlClient.SqlConnection
                {
                    string query = @"UPDATE Employee SET 
                                       FirstName = @FirstName, 
                                       LastName = @LastName, 
                                       DateOfBirth = @DateOfBirth, 
                                       Gender = @Gender, 
                                       Email = @Email, 
                                       PhoneNumber = @PhoneNumber, 
                                       Address = @Address, 
                                       Position = @Position, 
                                       JoiningDate = @JoiningDate, 
                                       TerminationDate = @TerminationDate
                                       WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeData.EmployeeID);
                        command.Parameters.AddWithValue("@FirstName", employeeData.FirstName);
                        command.Parameters.AddWithValue("@LastName", employeeData.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", employeeData.DateOfBirth);
                        command.Parameters.AddWithValue("@Gender", employeeData.Gender);
                        command.Parameters.AddWithValue("@Email", employeeData.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", employeeData.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", employeeData.Address);
                        command.Parameters.AddWithValue("@Position", employeeData.Position);
                        command.Parameters.AddWithValue("@JoiningDate", employeeData.JoiningDate);
                        command.Parameters.AddWithValue("@TerminationDate",
                            employeeData.TerminationDate.HasValue ? (object)employeeData.TerminationDate.Value : DBNull.Value);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (EmployeeNotFoundException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new InvalidInputException("Error updating employee: " + ex.Message);
            }
        }
    }
}