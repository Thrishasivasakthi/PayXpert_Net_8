using PayXpert_Net_8.Entity;
using System;
using System.Text.RegularExpressions;

namespace PayXpert_Net_8.Util
{
    public static class ValidationService
    {
        public static void ValidateEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FirstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(employee.LastName))
                throw new ArgumentException("Last name is required.");

            if (employee.DateOfBirth > DateTime.Now.AddYears(-18))
                throw new ArgumentException("Employee must be at least 18 years old.");

            if (string.IsNullOrWhiteSpace(employee.Email) || !IsValidEmail(employee.Email))
                throw new ArgumentException("Valid email is required.");

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber) || !IsValidPhoneNumber(employee.PhoneNumber))
                throw new ArgumentException("Valid phone number is required.");

            if (string.IsNullOrWhiteSpace(employee.Position))
                throw new ArgumentException("Position is required.");

            if (employee.JoiningDate > DateTime.Now)
                throw new ArgumentException("Joining date cannot be in the future.");

            if (employee.TerminationDate.HasValue && employee.TerminationDate < employee.JoiningDate)
                throw new ArgumentException("Termination date cannot be before joining date.");
        }

        public static void ValidateFinancialRecord(int employeeId, string description, decimal amount, string recordType)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.");

            if (string.IsNullOrWhiteSpace(recordType))
                throw new ArgumentException("Record type is required.");

            if (!recordType.Equals("Income", StringComparison.OrdinalIgnoreCase) &&
                !recordType.Equals("Expense", StringComparison.OrdinalIgnoreCase) &&
                !recordType.Equals("Tax", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Record type must be Income, Expense, or Tax.");
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^[0-9]{10,15}$");
        }
    }
}