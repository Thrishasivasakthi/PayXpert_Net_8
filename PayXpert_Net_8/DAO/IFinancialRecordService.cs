using PayXpert_Net_8.Entity;
using System;
using System.Collections.Generic;

namespace PayXpert_Net_8.DAO.Interfaces
{
    public interface IFinancialRecordService
    {
        void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType);
        FinancialRecord GetFinancialRecordById(int recordId);
        List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId);
        List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate);
    }
}