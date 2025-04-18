using PayXpert_Net_8.Entity;
using System.Collections.Generic;

namespace PayXpert_Net_8.DAO.Interfaces
{
    public interface ITaxService
    {
        Tax CalculateTax(int employeeId, int taxYear);
        Tax GetTaxById(int taxId);
        List<Tax> GetTaxesForEmployee(int employeeId);
        List<Tax> GetTaxesForYear(int taxYear);
    }
}