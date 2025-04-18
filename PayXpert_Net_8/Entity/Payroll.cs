using System;

namespace PayXpert_Net_8.Entity
{
    public class Payroll
    {
        public int PayrollID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime PayPeriodStartDate { get; set; }
        public DateTime PayPeriodEndDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }

        public Payroll() { }

        public Payroll(int payrollID, int employeeID, DateTime payPeriodStartDate,
                      DateTime payPeriodEndDate, decimal basicSalary, decimal overtimePay,
                      decimal deductions, decimal netSalary)
        {
            PayrollID = payrollID;
            EmployeeID = employeeID;
            PayPeriodStartDate = payPeriodStartDate;
            PayPeriodEndDate = payPeriodEndDate;
            BasicSalary = basicSalary;
            OvertimePay = overtimePay;
            Deductions = deductions;
            NetSalary = netSalary;
        }
    }
}