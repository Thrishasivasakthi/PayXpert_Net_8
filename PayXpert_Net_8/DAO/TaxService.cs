
using PayXpert_Net_8.Entity;
using PayXpert_Net_8.Exception;
using PayXpert_Net_8.Util;
using PayXpert_Net_8.DAO.Interfaces;
using PayXpert_Net_8.DAO.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PayXpert_Net_8.DAO.Services
{
    public class TaxService : ITaxService
    {
        private readonly string connectionString;

        public TaxService()
        {
            connectionString = DBPropertyUtil.GetConnectionString();
        }

        public Tax CalculateTax(int employeeId, int taxYear)
        {
            try
            {
                // Check if employee exists
                EmployeeService employeeService = new EmployeeService();
                Employee employee = employeeService.GetEmployeeById(employeeId);

                // Get all payrolls for the tax year
                DateTime startDate = new DateTime(taxYear, 1, 1);
                DateTime endDate = new DateTime(taxYear, 12, 31);

                PayrollService payrollService = new PayrollService();
                List<Payroll> payrolls = payrollService.GetPayrollsForEmployee(employeeId)
                    .FindAll(p => p.PayPeriodStartDate >= startDate && p.PayPeriodEndDate <= endDate);

                if (payrolls.Count == 0)
                {
                    throw new TaxCalculationException($"No payroll records found for employee {employeeId} in tax year {taxYear}");
                }

                // Calculate taxable income (sum of all net salaries for the year)
                decimal taxableIncome = 0;
                foreach (var payroll in payrolls)
                {
                    taxableIncome += payroll.NetSalary;
                }

                // Calculate tax (simplified for example)
                decimal taxAmount = CalculateTaxAmount(taxableIncome);




                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    connection.Open();
                    Tax tax = new Tax
                    {
                        TaxID = DBConnUtil.GetNextAvailableId("Tax", connection),
                        EmployeeID = employeeId,
                        TaxYear = taxYear,
                        TaxableIncome = taxableIncome,
                        TaxAmount = taxAmount
                    };

                    string query = @"INSERT INTO Tax (TaxID, EmployeeID, TaxYear, TaxableIncome, TaxAmount)
                                   VALUES (@TaxID, @EmployeeID, @TaxYear, @TaxableIncome, @TaxAmount)";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaxID", tax.TaxID);
                        command.Parameters.AddWithValue("@EmployeeID", tax.EmployeeID);
                        command.Parameters.AddWithValue("@TaxYear", tax.TaxYear);
                        command.Parameters.AddWithValue("@TaxableIncome", tax.TaxableIncome);
                        command.Parameters.AddWithValue("@TaxAmount", tax.TaxAmount);


                        command.ExecuteNonQuery();
                    }
                    return tax;
                }


            }
            catch (System.Exception ex)
            {
                throw new TaxCalculationException("Error calculating tax: " + ex.Message);
            }
        }

        private decimal CalculateTaxAmount(decimal taxableIncome)
        {
            // Simplified tax calculation (in a real app, this would use tax brackets)
            if (taxableIncome <= 10000)
                return taxableIncome * 0.10m;
            else if (taxableIncome <= 40000)
                return 1000 + (taxableIncome - 10000) * 0.15m;
            else
                return 5500 + (taxableIncome - 40000) * 0.20m;
        }

        public Tax GetTaxById(int taxId)
        {
            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM Tax WHERE TaxID = @TaxID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaxID", taxId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Tax
                                {
                                    TaxID = Convert.ToInt32(reader["TaxID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    TaxYear = Convert.ToInt32(reader["TaxYear"]),
                                    TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                                    TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                                };
                            }
                        }
                    }
                }
                throw new TaxCalculationException($"Tax with ID {taxId} not found.");
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving tax: " + ex.Message);
            }
        }

        public List<Tax> GetTaxesForEmployee(int employeeId)
        {
            List<Tax> taxes = new List<Tax>();

            try
            {
                // Check if employee exists
                EmployeeService employeeService = new EmployeeService();
                employeeService.GetEmployeeById(employeeId);

                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM Tax WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                taxes.Add(new Tax
                                {
                                    TaxID = Convert.ToInt32(reader["TaxID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    TaxYear = Convert.ToInt32(reader["TaxYear"]),
                                    TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                                    TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                                });
                            }
                        }
                    }
                }
                return taxes;
            }
            catch (EmployeeNotFoundException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving taxes: " + ex.Message);
            }
        }

        public List<Tax> GetTaxesForYear(int taxYear)
        {
            List<Tax> taxes = new List<Tax>();

            try
            {
                using (SqlConnection connection = DBConnUtil.GetConnection(connectionString))
                {
                    string query = "SELECT * FROM Tax WHERE TaxYear = @TaxYear";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaxYear", taxYear);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                taxes.Add(new Tax
                                {
                                    TaxID = Convert.ToInt32(reader["TaxID"]),
                                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                                    TaxYear = Convert.ToInt32(reader["TaxYear"]),
                                    TaxableIncome = Convert.ToDecimal(reader["TaxableIncome"]),
                                    TaxAmount = Convert.ToDecimal(reader["TaxAmount"])
                                });
                            }
                        }
                    }
                }
                return taxes;
            }
            catch (System.Exception ex)
            {
                throw new DatabaseConnectionException("Error retrieving taxes: " + ex.Message);
            }
        }
    }
}