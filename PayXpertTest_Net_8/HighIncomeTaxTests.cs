using NUnit.Framework;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using System;

namespace PayXpertTests
{
    [TestFixture]
    public class HighIncomeTaxTests
    {
        private PayrollService _payrollService;
        private EmployeeService _employeeService;
        private int _testEmployeeId;

        [SetUp]
        public void Setup()
        {
            _payrollService = new PayrollService();
            _employeeService = new EmployeeService();

            var employee = new Employee
            {
                FirstName = "Robert",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1982, 7, 8),
                Gender = "Male",
                Email = "robert.j@payxpert.com",
                PhoneNumber = "3456789012",
                Address = "789 Pine Rd, Chicago",
                Position = "Director",
                JoiningDate = new DateTime(2019, 7, 22),
                TerminationDate = null
            };
            _employeeService.AddEmployee(employee);
            _testEmployeeId = employee.EmployeeID;
        }

        [Test]
        public void VerifyTaxCalculationForHighIncomeEmployee_ReturnsCorrectTaxAmount()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 31);

            // Using high salary values
            decimal highBasicSalary = 10000.00m;
            decimal highOvertime = 2000.00m;
            decimal expectedTax = (highBasicSalary + highOvertime) * 0.15m;

            // Act
            var payroll = _payrollService.GeneratePayroll(_testEmployeeId, startDate, endDate);

            // Override with high salary values for test
            payroll.BasicSalary = highBasicSalary;
            payroll.OvertimePay = highOvertime;
            payroll.Deductions = _payrollService.CalculateDeductions(_testEmployeeId, highBasicSalary + highOvertime);
            payroll.NetSalary = highBasicSalary + highOvertime - payroll.Deductions;

            // Assert
            decimal actualTax = payroll.Deductions - 200.00m; // Subtract fixed deductions
            Assert.That(actualTax, Is.EqualTo(expectedTax).Within(0.01), "Tax calculation for high income is incorrect");
        }

        [TearDown]
        public void Cleanup()
        {
            try
            {
                _employeeService.RemoveEmployee(_testEmployeeId);
            }
            catch { /* Ignore cleanup errors */ }
        }
    }
}