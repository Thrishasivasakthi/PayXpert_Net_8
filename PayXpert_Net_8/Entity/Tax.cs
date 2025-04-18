namespace PayXpert_Net_8.Entity
{
    public class Tax
    {
        public int TaxID { get; set; }
        public int EmployeeID { get; set; }
        public int TaxYear { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxAmount { get; set; }

        public Tax() { }

        public Tax(int taxID, int employeeID, int taxYear, decimal taxableIncome, decimal taxAmount)
        {
            TaxID = taxID;
            EmployeeID = employeeID;
            TaxYear = taxYear;
            TaxableIncome = taxableIncome;
            TaxAmount = taxAmount;
        }
    }
}