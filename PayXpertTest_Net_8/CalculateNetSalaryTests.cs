using NUnit.Framework;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using System;

namespace PayXpertTests
{
    [TestFixture]
    public class CalculateNetSalaryTests
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
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1990, 8, 22),
                Gender = "Female",
                Email = "jane.smith@payxpert.com",
                PhoneNumber = "2345678901",
                Address = "456 Oak Ave, Boston",
                Position = "Senior Developer",
                JoiningDate = new DateTime(2021, 3, 15),
                TerminationDate = null
            };
            _employeeService.AddEmployee(employee);
            _testEmployeeId = employee.EmployeeID;
        }

        [Test]
        public void CalculateNetSalaryAfterDeductions_ValidInput_ReturnsCorrectNetSalary()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 31);
            decimal expectedDeductions = (3000m + 500m) * 0.15m + 200m; // Tax + other deductions
            decimal expectedNetSalary = 3500m - expectedDeductions;

            // Act
            var payroll = _payrollService.GeneratePayroll(_testEmployeeId, startDate, endDate);

            // Assert
            Assert.That(payroll.NetSalary, Is.EqualTo(expectedNetSalary).Within(0.01), "Net salary calculation is incorrect");
            Assert.That(payroll.Deductions, Is.EqualTo(expectedDeductions).Within(0.01), "Deductions calculation is incorrect");
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