namespace PayXpert_Net_8.Exception
{
    public class TaxCalculationException : System.Exception
    {
        public TaxCalculationException() : base("Error calculating tax.") { }
        public TaxCalculationException(string message) : base(message) { }
        public TaxCalculationException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}