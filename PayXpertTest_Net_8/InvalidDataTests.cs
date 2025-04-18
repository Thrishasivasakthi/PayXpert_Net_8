using NUnit.Framework;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using PayXpert_Net_8.Exception;
using System;

namespace PayXpertTests
{
    [TestFixture]
    public class InvalidDataTests
    {
        private PayrollService _payrollService;
        private EmployeeService _employeeService;
        private int _terminatedEmployeeId;

        [SetUp]
        public void Setup()
        {
            _payrollService = new PayrollService();
            _employeeService = new EmployeeService();

            // Create a terminated employee for testing
            var terminatedEmployee = new Employee
            {
                FirstName = "Terminated",
                LastName = "Employee",
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = "Male",
                Email = "terminated@payxpert.com",
                PhoneNumber = "9876543210",
                Address = "123 Terminated St",
                Position = "Former Employee",
                JoiningDate = new DateTime(2018, 1, 1),
                TerminationDate = DateTime.Now.AddDays(-30) // Terminated 30 days ago
            };
            _employeeService.AddEmployee(terminatedEmployee);
            _terminatedEmployeeId = terminatedEmployee.EmployeeID;
        }

        [Test]
        public void VerifyErrorHandlingForInvalidEmployeeData_ThrowsProperException()
        {
            // Arrange
            int invalidEmployeeId = -999; // Non-existent ID
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 31);

            // Act & Assert
            var ex = Assert.Throws<PayrollGenerationException>(() =>
                _payrollService.GeneratePayroll(invalidEmployeeId, startDate, endDate));

            Assert.That(ex.Message, Does.Contain("not found").IgnoreCase,
                "Error message should indicate employee not found");
        }

       

        [TearDown]
        public void Cleanup()
        {
            try
            {
                _employeeService.RemoveEmployee(_terminatedEmployeeId);
            }
            catch { /* Ignore cleanup errors */ }
        }
    }
}