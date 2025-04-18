using System;

namespace PayXpert_Net_8.Entity
{
    public class FinancialRecord
    {
        public int RecordID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime RecordDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string RecordType { get; set; }

        public FinancialRecord() { }

        public FinancialRecord(int recordID, int employeeID, DateTime recordDate,
                              string description, decimal amount, string recordType)
        {
            RecordID = recordID;
            EmployeeID = employeeID;
            RecordDate = recordDate;
            Description = description;
            Amount = amount;
            RecordType = recordType;
        }
    }
}