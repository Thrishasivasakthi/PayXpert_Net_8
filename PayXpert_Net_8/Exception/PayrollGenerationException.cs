namespace PayXpert_Net_8.Exception
{
    public class PayrollGenerationException : System.Exception
    {
        public PayrollGenerationException() : base("Error generating payroll.") { }
        public PayrollGenerationException(string message) : base(message) { }
        public PayrollGenerationException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}