using PayXpert_Net_8.DAO.Interfaces;
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
    public class PayrollService : IPayrollService
    {
        private readonly string connectionString;

        public PayrollService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public Payroll GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate date range
                if (endDate <= startDate)
                    throw new InvalidInputException("End date must be after start date");

                if ((endDate - startDate).TotalDays > 31)
                    throw new InvalidInputException("Pay period cannot exceed 31 days");

                // Check if employee exists
                EmployeeService employeeService = new EmployeeService();
                Employee employee = employeeService.GetEmployeeById(employeeId);

                if (employee.TerminationDate.HasValue && employee.TerminationDate.Value < endDate)
                    throw new PayrollGenerationException("Cannot generate payroll for terminated employee");

                // Check for existing payroll for same period
                if (HasExistingPayroll(employeeId, startDate, endDate))
                    throw new PayrollGenerationException("Payroll already exists for this period");

                // Calculate payroll components
                decimal basicSalary = CalculateBasicSalary(employeeId);
                decimal overtimePay = CalculateOvertimePay(employeeId, startDate, endDate);
                decimal deductions = CalculateDeductions(employeeId, basicSalary + overtimePay);
                decimal netSalary = basicSalary + overtimePay - deductions;

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    connection.Open();

                    // Get next available ID
                    int payrollId = GetNextPayrollId(connection);

                    // Create payroll object
                    Payroll payroll = new Payroll
                    {
                        PayrollID = payrollId,
                        EmployeeID = employeeId,
                        PayPeriodStartDate = startDate,
                        PayPeriodEndDate = endDate,
                        BasicSalary = basicSalary,
                        OvertimePay = overtimePay,
                        Deductions = deductions,
                        NetSalary = netSalary
                    };

                    // Insert payroll record
                    string query = @"INSERT INTO Payroll 
                                   (PayrollID, EmployeeID, PayPeriodStartDate, PayPeriodEndDate,
                                    BasicSalary, OvertimePay, Deductions, NetSalary)
                                   VALUES 
                                   (@PayrollID, @EmployeeID, @PayPeriodStartDate, @PayPeriodEndDate,
                                    @BasicSalary, @OvertimePay, @Deductions, @NetSalary)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PayrollID", payroll.PayrollID);
                        command.Parameters.AddWithValue("@EmployeeID", payroll.EmployeeID);
                        command.Parameters.AddWithValue("@PayPeriodStartDate", payroll.PayPeriodStartDate);
                        command.Parameters.AddWithValue("@PayPeriodEndDate", payroll.PayPeriodEndDate);
                        command.Parameters.AddWithValue("@BasicSalary", payroll.BasicSalary);
                        command.Parameters.AddWithValue("@OvertimePay", payroll.OvertimePay);
                        command.Parameters.AddWithValue("@Deductions", payroll.Deductions);
                        command.Parameters.AddWithValue("@NetSalary", payroll.NetSalary);

                        command.ExecuteNonQuery();
                    }

                    return payroll;
                }
            }
            catch (System.Exception ex)
            {
                throw new PayrollGenerationException("Error generating payroll: " + ex.Message);
            }
        }

        private bool HasExistingPayroll(int employeeId, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM Payroll 
                               WHERE EmployeeID = @EmployeeID
                               AND PayPeriodStartDate = @StartDate
                               AND PayPeriodEndDate = @EndDate";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        private int GetNextPayrollId(SqlConnection connection)
        {
            string query = "SELECT ISNULL(MAX(PayrollID), 0) + 1 FROM Payroll";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                return Convert.ToInt32(result == DBNull.Value ? 1 : result);
            }
        }

        private decimal CalculateBasicSalary(int employeeId)
        {

            return 3000.00m;
        }

        public decimal CalculateOvertimePay(int employeeId, DateTime startDate, DateTime endDate)
        {

            return 500.00m;
        }

        public decimal CalculateDeductions(int employeeId, decimal grossSalary)
        {
            decimal tax = grossSalary * 0.15m;
            decimal otherDeductions = 200.00m;
            return tax + otherDeductions;
        }

        public Payroll GetPayrollById(int payrollId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM Payroll WHERE PayrollID = @PayrollID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PayrollID", payrollId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Payroll
                                {
                                    PayrollID = Convert.ToInt32(reader["PayrollID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                                    PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                                    BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                                    OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                                    Deductions = Convert.ToDecimal(reader["Deductions"]),
                                    NetSalary = Convert.ToDecimal(reader["NetSalary"])
                                };
                            }
                        }
                    }
                }
                throw new PayrollGenerationException($"Payroll with ID {payrollId} not found.");
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving payroll: " + ex.Message);
            }
        }

        public List<Payroll> GetPayrollsForEmployee(int employeeId)
        {
            List<Payroll> payrolls = new List<Payroll>();

            try
            {
                // Verify employee exists
                new EmployeeService().GetEmployeeById(employeeId);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM Payroll WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payrolls.Add(new Payroll
                                {
                                    PayrollID = Convert.ToInt32(reader["PayrollID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                                    PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                                    BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                                    OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                                    Deductions = Convert.ToDecimal(reader["Deductions"]),
                                    NetSalary = Convert.ToDecimal(reader["NetSalary"])
                                });
                            }
                        }
                    }
                }
                return payrolls;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving payrolls: " + ex.Message);
            }
        }

        public List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate)
        {
            List<Payroll> payrolls = new List<Payroll>();

            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = @"SELECT * FROM Payroll 
                                   WHERE PayPeriodStartDate <= @EndDate 
                                   AND PayPeriodEndDate >= @StartDate";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payrolls.Add(new Payroll
                                {
                                    PayrollID = Convert.ToInt32(reader["PayrollID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    PayPeriodStartDate = Convert.ToDateTime(reader["PayPeriodStartDate"]),
                                    PayPeriodEndDate = Convert.ToDateTime(reader["PayPeriodEndDate"]),
                                    BasicSalary = Convert.ToDecimal(reader["BasicSalary"]),
                                    OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
                                    Deductions = Convert.ToDecimal(reader["Deductions"]),
                                    NetSalary = Convert.ToDecimal(reader["NetSalary"])
                                });
                            }
                        }
                    }
                }
                return payrolls;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving payrolls: " + ex.Message);
            }
        }
    }
}