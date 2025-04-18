using PayXpert_Net_8.Entity;
using System.Collections.Generic;

namespace PayXpert_Net_8.DAO.Interfaces
{
    public interface IEmployeeService
    {
        Employee GetEmployeeById(int employeeId);
        List<Employee> GetAllEmployees();
        void AddEmployee(Employee employeeData);
        void UpdateEmployee(Employee employeeData);
        void RemoveEmployee(int employeeId);
    }
}