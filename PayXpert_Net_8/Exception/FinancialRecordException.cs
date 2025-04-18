namespace PayXpert_Net_8.Exception
{
    public class FinancialRecordException : System.Exception
    {
        public FinancialRecordException() : base("Error with financial record.") { }
        public FinancialRecordException(string message) : base(message) { }
        public FinancialRecordException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}