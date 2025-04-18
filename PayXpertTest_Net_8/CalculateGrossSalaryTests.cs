using NUnit.Framework;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using System;

namespace PayXpertTests
{
    [TestFixture]
    public class CalculateGrossSalaryTests
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
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = "Male",
                Email = "john.doe@payxpert.com",
                PhoneNumber = "1234567890",
                Address = "123 Main St, New York",
                Position = "Software Engineer",
                JoiningDate = new DateTime(2020, 1, 10),
                TerminationDate = null
            };
            _employeeService.AddEmployee(employee);
            _testEmployeeId = employee.EmployeeID;
        }

        [Test]
        public void CalculateGrossSalaryForEmployee_ValidInput_ReturnsCorrectGrossSalary()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 31);

            // Act
            var payroll = _payrollService.GeneratePayroll(_testEmployeeId, startDate, endDate);
            decimal grossSalary = payroll.BasicSalary + payroll.OvertimePay;

            // Assert
            Assert.AreEqual(3500.00m, grossSalary, "Gross salary should be sum of basic salary (3000) and overtime pay (500)");
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