using PayXpert_Net_8.Entity;
using System;
using System.Collections.Generic;

namespace PayXpert_Net_8.Util
{
    public static class ReportGenerator
    {
        public static void GeneratePayrollReport(List<Payroll> payrolls)
        {
            Console.WriteLine("\nPayroll Report");
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("| ID  | Employee ID | Period Start | Period End  | Net Salary  |");
            Console.WriteLine("-----------------------------------------------------------------");

            foreach (var payroll in payrolls)
            {
                Console.WriteLine($"| {payroll.PayrollID,-4} | {payroll.EmployeeID,-11} | " +
                    $"{payroll.PayPeriodStartDate:yyyy-MM-dd} | {payroll.PayPeriodEndDate:yyyy-MM-dd} | " +
                    $"{payroll.NetSalary,11:C} |");
            }

            Console.WriteLine("-----------------------------------------------------------------");
        }

        public static void GenerateTaxReport(List<Tax> taxes)
        {
            Console.WriteLine("\nTax Report");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("| ID  | Employee ID | Tax Year | Taxable Income | Tax Amount |");
            Console.WriteLine("---------------------------------------------------------------");

            foreach (var tax in taxes)
            {
                Console.WriteLine($"| {tax.TaxID,-4} | {tax.EmployeeID,-11} | {tax.TaxYear,-9} | " +
                    $"{tax.TaxableIncome,14:C} | {tax.TaxAmount,10:C} |");
            }

            Console.WriteLine("---------------------------------------------------------------");
        }

        public static void GenerateFinancialRecordReport(List<FinancialRecord> records)
        {
            Console.WriteLine("\nFinancial Records Report");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("| ID  | Employee ID | Record Date | Description           | Amount      |");
            Console.WriteLine("---------------------------------------------------------------------------");

            foreach (var record in records)
            {
                Console.WriteLine($"| {record.RecordID,-4} | {record.EmployeeID,-11} | " +
                    $"{record.RecordDate:yyyy-MM-dd} | {record.Description,-22} | {record.Amount,11:C} |");
            }

            Console.WriteLine("---------------------------------------------------------------------------");
        }
    }
}