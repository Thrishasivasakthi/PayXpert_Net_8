namespace PayXpert_Net_8.Exception
{
    public class DatabaseConnectionException : System.Exception
    {
        public DatabaseConnectionException() : base("Database connection error.") { }
        public DatabaseConnectionException(string message) : base(message) { }
        public DatabaseConnectionException(string message, System.Exception inner)
            : base(message, inner) { }
    }
}