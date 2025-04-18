using NUnit.Framework;
using PayXpert_Net_8.DAO.Services;
using PayXpert_Net_8.Entity;
using System;

namespace PayXpertTests
{
    [TestFixture]
    public class MultipleEmployeesTests
    {
        private PayrollService _payrollService;
        private EmployeeService _employeeService;
        private int _employeeId1, _employeeId2;

        [SetUp]
        public void Setup()
        {
            _payrollService = new PayrollService();
            _employeeService = new EmployeeService();

            var emp1 = new Employee
            {
                FirstName = "Alice",
                LastName = "Williams",
                DateOfBirth = new DateTime(1992, 3, 10),
                Gender = "Female",
                Email = "alice.w@payxpert.com",
                PhoneNumber = "4567890123",
                Address = "321 Elm St, Seattle",
                Position = "HR Manager",
                JoiningDate = new DateTime(2022, 2, 1),
                TerminationDate = null
            };

            var emp2 = new Employee
            {
                FirstName = "Michael",
                LastName = "Brown",
                DateOfBirth = new DateTime(1988, 11, 30),
                Gender = "Male",
                Email = "michael.b@payxpert.com",
                PhoneNumber = "5678901234",
                Address = "654 Maple Dr, Austin",
                Position = "Finance Analyst",
                JoiningDate = new DateTime(2021, 5, 18),
                TerminationDate = null
            };

            _employeeService.AddEmployee(emp1);
            _employeeService.AddEmployee(emp2);
            _employeeId1 = emp1.EmployeeID;
            _employeeId2 = emp2.EmployeeID;
        }

        [Test]
        public void ProcessPayrollForMultipleEmployees_CompletesSuccessfully()
        {
            // Arrange
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 1, 31);

            // Act
            var payroll1 = _payrollService.GeneratePayroll(_employeeId1, startDate, endDate);
            var payroll2 = _payrollService.GeneratePayroll(_employeeId2, startDate, endDate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(payroll1, "First payroll should be generated");
                Assert.IsNotNull(payroll2, "Second payroll should be generated");
                Assert.AreNotEqual(payroll1.PayrollID, payroll2.PayrollID, "Payroll IDs should be unique");
                Assert.AreEqual(_employeeId1, payroll1.EmployeeID, "First payroll should match first employee");
                Assert.AreEqual(_employeeId2, payroll2.EmployeeID, "Second payroll should match second employee");
            });
        }

        [TearDown]
        public void Cleanup()
        {
            try
            {
                _employeeService.RemoveEmployee(_employeeId1);
                _employeeService.RemoveEmployee(_employeeId2);
            }
            catch { /* Ignore cleanup errors */ }
        }
    }
}