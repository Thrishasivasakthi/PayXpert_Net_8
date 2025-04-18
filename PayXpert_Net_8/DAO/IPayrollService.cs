using PayXpert_Net_8.Entity;
using System;
using System.Collections.Generic;

namespace PayXpert_Net_8.DAO.Interfaces
{
    public interface IPayrollService
    {
        Payroll GeneratePayroll(int employeeId, DateTime startDate, DateTime endDate);
        Payroll GetPayrollById(int payrollId);
        List<Payroll> GetPayrollsForEmployee(int employeeId);
        List<Payroll> GetPayrollsForPeriod(DateTime startDate, DateTime endDate);
    }
}