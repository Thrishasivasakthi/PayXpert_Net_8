using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using PayXpert_Net_8.Exception;
using PayXpert_Net_8.Util;
using System;
using System.Collections.Generic;

namespace PayXpert.Main
{
    public class MainModule
    {
        private readonly EmployeeService employeeService;
        private readonly PayrollService payrollService;
        private readonly TaxService taxService;
        private readonly FinancialRecordService financialRecordService;

        public MainModule()
        {
            employeeService = new EmployeeService();
            payrollService = new PayrollService();
            taxService = new TaxService();
            financialRecordService = new FinancialRecordService();
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nPayXpert - Payroll Management System");
                Console.WriteLine("------------------------------------");
                Console.WriteLine("1. Employee Management");
                Console.WriteLine("2. Payroll Processing");
                Console.WriteLine("3. Tax Calculation");
                Console.WriteLine("4. Financial Records");
                Console.WriteLine("5. Reports");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                EmployeeManagementMenu();
                                break;
                            case 2:
                                PayrollProcessingMenu();
                                break;
                            case 3:
                                TaxCalculationMenu();
                                break;
                            case 4:
                                FinancialRecordsMenu();
                                break;
                            case 5:
                                ReportsMenu();
                                break;
                            case 6:
                                exit = true;
                                Console.WriteLine("Exiting PayXpert. Goodbye!");
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void EmployeeManagementMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nEmployee Management");
                Console.WriteLine("-------------------");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Update Employee");
                Console.WriteLine("3. Remove Employee");
                Console.WriteLine("4. Get Employee by ID");
                Console.WriteLine("5. Get All Employees");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddEmployee();
                                break;
                            case 2:
                                UpdateEmployee();
                                break;
                            case 3:
                                RemoveEmployee();
                                break;
                            case 4:
                                GetEmployeeById();
                                break;
                            case 5:
                                GetAllEmployees();
                                break;
                            case 6:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void AddEmployee()
        {
            Console.WriteLine("\nAdd New Employee");
            Console.WriteLine("----------------");

            Employee employee = new Employee();

            Console.Write("First Name: ");
            employee.FirstName = Console.ReadLine();

            Console.Write("Last Name: ");
            employee.LastName = Console.ReadLine();

            Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                employee.DateOfBirth = dob;
            else
                throw new InvalidInputException("Invalid date format.");

            Console.Write("Gender: ");
            employee.Gender = Console.ReadLine();

            Console.Write("Email: ");
            employee.Email = Console.ReadLine();

            Console.Write("Phone Number: ");
            employee.PhoneNumber = Console.ReadLine();

            Console.Write("Address: ");
            employee.Address = Console.ReadLine();

            Console.Write("Position: ");
            employee.Position = Console.ReadLine();

            Console.Write("Joining Date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime joiningDate))
                employee.JoiningDate = joiningDate;
            else
                throw new InvalidInputException("Invalid date format.");

            Console.Write("Termination Date (yyyy-MM-dd, optional - press Enter to skip): ");
            string terminationDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(terminationDateInput))
            {
                if (DateTime.TryParse(terminationDateInput, out DateTime terminationDate))
                    employee.TerminationDate = terminationDate;
                else
                    throw new InvalidInputException("Invalid date format.");
            }

            employeeService.AddEmployee(employee);
            Console.WriteLine("Employee added successfully!");
        }

        private void UpdateEmployee()
        {
            Console.WriteLine("\nUpdate Employee");
            Console.WriteLine("---------------");

            Console.Write("Enter Employee ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Employee existingEmployee = employeeService.GetEmployeeById(employeeId);
            Console.WriteLine($"\nCurrent Details for Employee ID {employeeId}:");
            DisplayEmployeeDetails(existingEmployee);

            Employee updatedEmployee = new Employee
            {
                EmployeeID = employeeId,
                FirstName = existingEmployee.FirstName,
                LastName = existingEmployee.LastName,
                DateOfBirth = existingEmployee.DateOfBirth,
                Gender = existingEmployee.Gender,
                Email = existingEmployee.Email,
                PhoneNumber = existingEmployee.PhoneNumber,
                Address = existingEmployee.Address,
                Position = existingEmployee.Position,
                JoiningDate = existingEmployee.JoiningDate,
                TerminationDate = existingEmployee.TerminationDate
            };

            Console.WriteLine("\nEnter new details (press Enter to keep current value):");

            Console.Write($"First Name ({existingEmployee.FirstName}): ");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName)) updatedEmployee.FirstName = firstName;

            Console.Write($"Last Name ({existingEmployee.LastName}): ");
            string lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName)) updatedEmployee.LastName = lastName;

            Console.Write($"Date of Birth ({existingEmployee.DateOfBirth:yyyy-MM-dd}): ");
            string dobInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dobInput) && DateTime.TryParse(dobInput, out DateTime dob))
                updatedEmployee.DateOfBirth = dob;

            Console.Write($"Gender ({existingEmployee.Gender}): ");
            string gender = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(gender)) updatedEmployee.Gender = gender;

            Console.Write($"Email ({existingEmployee.Email}): ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) updatedEmployee.Email = email;

            Console.Write($"Phone Number ({existingEmployee.PhoneNumber}): ");
            string phoneNumber = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phoneNumber)) updatedEmployee.PhoneNumber = phoneNumber;

            Console.Write($"Address ({existingEmployee.Address}): ");
            string address = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(address)) updatedEmployee.Address = address;

            Console.Write($"Position ({existingEmployee.Position}): ");
            string position = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(position)) updatedEmployee.Position = position;

            Console.Write($"Joining Date ({existingEmployee.JoiningDate:yyyy-MM-dd}): ");
            string joiningDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(joiningDateInput) && DateTime.TryParse(joiningDateInput, out DateTime joiningDate))
                updatedEmployee.JoiningDate = joiningDate;

            Console.Write($"Termination Date ({(existingEmployee.TerminationDate.HasValue ? existingEmployee.TerminationDate.Value.ToString("yyyy-MM-dd") : "null")}): ");
            string terminationDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(terminationDateInput))
            {
                if (DateTime.TryParse(terminationDateInput, out DateTime terminationDate))
                    updatedEmployee.TerminationDate = terminationDate;
                else
                    updatedEmployee.TerminationDate = null;
            }
            else if (terminationDateInput == string.Empty)
            {
                updatedEmployee.TerminationDate = null;
            }

            employeeService.UpdateEmployee(updatedEmployee);
            Console.WriteLine("Employee updated successfully!");
        }

        private void RemoveEmployee()
        {
            Console.WriteLine("\nRemove Employee");
            Console.WriteLine("---------------");

            Console.Write("Enter Employee ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Employee employee = employeeService.GetEmployeeById(employeeId);
            Console.WriteLine($"\nDetails for Employee ID {employeeId}:");
            DisplayEmployeeDetails(employee);

            Console.Write("Are you sure you want to remove this employee? (y/n): ");
            string confirmation = Console.ReadLine();
            if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                employeeService.RemoveEmployee(employeeId);
                Console.WriteLine("Employee removed successfully!");
            }
            else
            {
                Console.WriteLine("Employee removal cancelled.");
            }
        }

        private void GetEmployeeById()
        {
            Console.WriteLine("\nGet Employee by ID");
            Console.WriteLine("------------------");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Employee employee = employeeService.GetEmployeeById(employeeId);
            DisplayEmployeeDetails(employee);
        }

        private void GetAllEmployees()
        {
            Console.WriteLine("\nAll Employees");
            Console.WriteLine("-------------");

            List<Employee> employees = employeeService.GetAllEmployees();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine("| ID  | Name                | Position        | Email                     | Phone       | Age  |");
            Console.WriteLine("--------------------------------------------------------------------------------------------------");

            foreach (var employee in employees)
            {
                Console.WriteLine($"| {employee.EmployeeID,-4} | {employee.FirstName + " " + employee.LastName,-19} | " +
                    $"{employee.Position,-15} | {employee.Email,-25} | {employee.PhoneNumber,-11} | {employee.CalculateAge(),-4} |");
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------------");
        }

        private void DisplayEmployeeDetails(Employee employee)
        {
            Console.WriteLine($"\nEmployee ID: {employee.EmployeeID}");
            Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
            Console.WriteLine($"Date of Birth: {employee.DateOfBirth:yyyy-MM-dd} (Age: {employee.CalculateAge()})");
            Console.WriteLine($"Gender: {employee.Gender}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Phone: {employee.PhoneNumber}");
            Console.WriteLine($"Address: {employee.Address}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine($"Joining Date: {employee.JoiningDate:yyyy-MM-dd}");
            Console.WriteLine($"Termination Date: {(employee.TerminationDate.HasValue ? employee.TerminationDate.Value.ToString("yyyy-MM-dd") : "N/A")}");
        }

        private void PayrollProcessingMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nPayroll Processing");
                Console.WriteLine("------------------");
                Console.WriteLine("1. Generate Payroll");
                Console.WriteLine("2. Get Payroll by ID");
                Console.WriteLine("3. Get Payrolls for Employee");
                Console.WriteLine("4. Get Payrolls for Period");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                GeneratePayroll();
                                break;
                            case 2:
                                GetPayrollById();
                                break;
                            case 3:
                                GetPayrollsForEmployee();
                                break;
                            case 4:
                                GetPayrollsForPeriod();
                                break;
                            case 5:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void GeneratePayroll()
        {
            Console.WriteLine("\nGenerate Payroll");
            Console.WriteLine("----------------");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Console.Write("Enter Pay Period Start Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                throw new InvalidInputException("Invalid date format.");

            Console.Write("Enter Pay Period End Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                throw new InvalidInputException("Invalid date format.");

            Payroll payroll = payrollService.GeneratePayroll(employeeId, startDate, endDate);
            Console.WriteLine("\nPayroll generated successfully!");
            DisplayPayrollDetails(payroll);
        }

        private void GetPayrollById()
        {
            Console.WriteLine("\nGet Payroll by ID");
            Console.WriteLine("-----------------");

            Console.Write("Enter Payroll ID: ");
            if (!int.TryParse(Console.ReadLine(), out int payrollId))
                throw new InvalidInputException("Invalid Payroll ID.");

            Payroll payroll = payrollService.GetPayrollById(payrollId);
            DisplayPayrollDetails(payroll);
        }

        private void GetPayrollsForEmployee()
        {
            Console.WriteLine("\nGet Payrolls for Employee");
            Console.WriteLine("-------------------------");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            List<Payroll> payrolls = payrollService.GetPayrollsForEmployee(employeeId);
            ReportGenerator.GeneratePayrollReport(payrolls);
        }

        private void GetPayrollsForPeriod()
        {
            Console.WriteLine("\nGet Payrolls for Period");
            Console.WriteLine("-----------------------");

            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                throw new InvalidInputException("Invalid date format.");

            Console.Write("Enter End Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                throw new InvalidInputException("Invalid date format.");

            List<Payroll> payrolls = payrollService.GetPayrollsForPeriod(startDate, endDate);
            ReportGenerator.GeneratePayrollReport(payrolls);
        }

        private void DisplayPayrollDetails(Payroll payroll)
        {
            Console.WriteLine($"\nPayroll ID: {payroll.PayrollID}");
            Console.WriteLine($"Employee ID: {payroll.EmployeeID}");
            Console.WriteLine($"Pay Period: {payroll.PayPeriodStartDate:yyyy-MM-dd} to {payroll.PayPeriodEndDate:yyyy-MM-dd}");
            Console.WriteLine($"Basic Salary: {payroll.BasicSalary:C}");
            Console.WriteLine($"Overtime Pay: {payroll.OvertimePay:C}");
            Console.WriteLine($"Deductions: {payroll.Deductions:C}");
            Console.WriteLine($"Net Salary: {payroll.NetSalary:C}");
        }

        private void TaxCalculationMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nTax Calculation");
                Console.WriteLine("---------------");
                Console.WriteLine("1. Calculate Tax");
                Console.WriteLine("2. Get Tax by ID");
                Console.WriteLine("3. Get Taxes for Employee");
                Console.WriteLine("4. Get Taxes for Year");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                CalculateTax();
                                break;
                            case 2:
                                GetTaxById();
                                break;
                            case 3:
                                GetTaxesForEmployee();
                                break;
                            case 4:
                                GetTaxesForYear();
                                break;
                            case 5:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void CalculateTax()
        {
            Console.WriteLine("\nCalculate Tax");
            Console.WriteLine("-------------");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Console.Write("Enter Tax Year: ");
            if (!int.TryParse(Console.ReadLine(), out int taxYear))
                throw new InvalidInputException("Invalid Tax Year.");

            Tax tax = taxService.CalculateTax(employeeId, taxYear);
            Console.WriteLine("\nTax calculated successfully!");
            DisplayTaxDetails(tax);
        }

        private void GetTaxById()
        {
            Console.WriteLine("\nGet Tax by ID");
            Console.WriteLine("-------------");

            Console.Write("Enter Tax ID: ");
            if (!int.TryParse(Console.ReadLine(), out int taxId))
                throw new InvalidInputException("Invalid Tax ID.");

            Tax tax = taxService.GetTaxById(taxId);
            DisplayTaxDetails(tax);
        }

        private void GetTaxesForEmployee()
        {
            Console.WriteLine("\nGet Taxes for Employee");
            Console.WriteLine("----------------------");

            Console.Write("Enter Employee ID: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            List<Tax> taxes = taxService.GetTaxesForEmployee(employeeId);
            ReportGenerator.GenerateTaxReport(taxes);
        }

        private void GetTaxesForYear()
        {
            Console.WriteLine("\nGet Taxes for Year");
            Console.WriteLine("------------------");

            Console.Write("Enter Tax Year: ");
            if (!int.TryParse(Console.ReadLine(), out int taxYear))
                throw new InvalidInputException("Invalid Tax Year.");

            List<Tax> taxes = taxService.GetTaxesForYear(taxYear);
            ReportGenerator.GenerateTaxReport(taxes);
        }

        private void DisplayTaxDetails(Tax tax)
        {
            Console.WriteLine($"\nTax ID: {tax.TaxID}");
            Console.WriteLine($"Employee ID: {tax.EmployeeID}");
            Console.WriteLine($"Tax Year: {tax.TaxYear}");
            Console.WriteLine($"Taxable Income: {tax.TaxableIncome:C}");
            Console.WriteLine($"Tax Amount: {tax.TaxAmount:C}");
        }

        private void FinancialRecordsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nFinancial Records");
                Console.WriteLine("-----------------");
                Console.WriteLine("1. Add Financial Record");
                Console.WriteLine("2. Get Financial Record by ID");
                Console.WriteLine("3. Get Financial Records for Employee");
                Console.WriteLine("4. Get Financial Records for Date");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                AddFinancialRecord();
                                break;
                            case 2:
                                GetFinancialRecordById();
                                break;
                            case 3:
                                GetFinancialRecordsForEmployee();
                                break;
                            case 4:
                                GetFinancialRecordsForDate();
                                break;
                            case 5:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void AddFinancialRecord()
        {
            Console.WriteLine("\nAdd Financial Record");
            Console.WriteLine("--------------------");

            Console.Write("Enter Employee ID (0 for company record): ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            Console.Write("Description: ");
            string description = Console.ReadLine();

            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                throw new InvalidInputException("Invalid amount.");

            Console.Write("Record Type (Income/Expense/Tax): ");
            string recordType = Console.ReadLine();

            financialRecordService.AddFinancialRecord(employeeId, description, amount, recordType);
            Console.WriteLine("Financial record added successfully!");
        }

        private void GetFinancialRecordById()
        {
            Console.WriteLine("\nGet Financial Record by ID");
            Console.WriteLine("--------------------------");

            Console.Write("Enter Record ID: ");
            if (!int.TryParse(Console.ReadLine(), out int recordId))
                throw new InvalidInputException("Invalid Record ID.");

            FinancialRecord record = financialRecordService.GetFinancialRecordById(recordId);
            DisplayFinancialRecordDetails(record);
        }

        private void GetFinancialRecordsForEmployee()
        {
            Console.WriteLine("\nGet Financial Records for Employee");
            Console.WriteLine("----------------------------------");

            Console.Write("Enter Employee ID (0 for company records): ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                throw new InvalidInputException("Invalid Employee ID.");

            List<FinancialRecord> records = financialRecordService.GetFinancialRecordsForEmployee(employeeId);
            ReportGenerator.GenerateFinancialRecordReport(records);
        }

        private void GetFinancialRecordsForDate()
        {
            Console.WriteLine("\nGet Financial Records for Date");
            Console.WriteLine("------------------------------");

            Console.Write("Enter Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime recordDate))
                throw new InvalidInputException("Invalid date format.");

            List<FinancialRecord> records = financialRecordService.GetFinancialRecordsForDate(recordDate);
            ReportGenerator.GenerateFinancialRecordReport(records);
        }

        private void DisplayFinancialRecordDetails(FinancialRecord record)
        {
            Console.WriteLine($"\nRecord ID: {record.RecordID}");
            Console.WriteLine($"Employee ID: {(record.EmployeeID > 0 ? record.EmployeeID.ToString() : "Company")}");
            Console.WriteLine($"Record Date: {record.RecordDate:yyyy-MM-dd}");
            Console.WriteLine($"Description: {record.Description}");
            Console.WriteLine($"Amount: {record.Amount:C}");
            Console.WriteLine($"Record Type: {record.RecordType}");
        }

        private void ReportsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\nReports");
                Console.WriteLine("-------");
                Console.WriteLine("1. Employee Report");
                Console.WriteLine("2. Payroll Report");
                Console.WriteLine("3. Tax Report");
                Console.WriteLine("4. Financial Records Report");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                GetAllEmployees(); // Reusing existing method
                                break;
                            case 2:
                                GeneratePayrollReport();
                                break;
                            case 3:
                                GenerateTaxReport();
                                break;
                            case 4:
                                GenerateFinancialRecordsReport();
                                break;
                            case 5:
                                back = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void GeneratePayrollReport()
        {
            Console.WriteLine("\nGenerate Payroll Report");
            Console.WriteLine("----------------------");
            Console.WriteLine("1. Payrolls for Employee");
            Console.WriteLine("2. Payrolls for Period");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    List<Payroll> payrolls;
                    switch (choice)
                    {

                        case 1:
                            Console.Write("Enter Employee ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                                throw new InvalidInputException("Invalid Employee ID.");
                            payrolls = payrollService.GetPayrollsForEmployee(employeeId);
                            break;
                        case 2:
                            Console.Write("Enter Start Date (yyyy-MM-dd): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                                throw new InvalidInputException("Invalid date format.");
                            Console.Write("Enter End Date (yyyy-MM-dd): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                                throw new InvalidInputException("Invalid date format.");
                            payrolls = payrollService.GetPayrollsForPeriod(startDate, endDate);
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            return;
                    }
                    ReportGenerator.GeneratePayrollReport(payrolls);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        private void GenerateTaxReport()
        {
            Console.WriteLine("\nGenerate Tax Report");
            Console.WriteLine("-------------------");
            Console.WriteLine("1. Taxes for Employee");
            Console.WriteLine("2. Taxes for Year");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    List<Tax> taxes;
                    switch (choice)
                    {

                        case 1:
                            Console.Write("Enter Employee ID: ");
                            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                                throw new InvalidInputException("Invalid Employee ID.");
                            taxes = taxService.GetTaxesForEmployee(employeeId);
                            break;
                        case 2:
                            Console.Write("Enter Tax Year: ");
                            if (!int.TryParse(Console.ReadLine(), out int taxYear))
                                throw new InvalidInputException("Invalid Tax Year.");
                            taxes = taxService.GetTaxesForYear(taxYear);
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            return;
                    }
                    ReportGenerator.GenerateTaxReport(taxes);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        private void GenerateFinancialRecordsReport()
        {
            Console.WriteLine("\nGenerate Financial Records Report");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1. Financial Records for Employee");
            Console.WriteLine("2. Financial Records for Date");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    List<FinancialRecord> records;
                    switch (choice)
                    {

                        case 1:
                            Console.Write("Enter Employee ID (0 for company records): ");
                            if (!int.TryParse(Console.ReadLine(), out int employeeId))
                                throw new InvalidInputException("Invalid Employee ID.");
                            records = financialRecordService.GetFinancialRecordsForEmployee(employeeId);
                            break;
                        case 2:
                            Console.Write("Enter Date (yyyy-MM-dd): ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime recordDate))
                                throw new InvalidInputException("Invalid date format.");
                            records = financialRecordService.GetFinancialRecordsForDate(recordDate);
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            return;
                    }
                    ReportGenerator.GenerateFinancialRecordReport(records);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }
}