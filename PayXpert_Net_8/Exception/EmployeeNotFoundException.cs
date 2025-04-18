namespace PayXpert_Net_8.Exception
{
    public class EmployeeNotFoundException : System.Exception
    {
        public EmployeeNotFoundException() : base("Employee not found.") { }
        public EmployeeNotFoundException(string message) : base(message) { }
        public EmployeeNotFoundException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}