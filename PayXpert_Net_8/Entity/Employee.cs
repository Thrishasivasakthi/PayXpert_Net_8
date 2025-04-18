using System;

namespace PayXpert_Net_8.Entity
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public Employee() { }

        public Employee(int employeeID, string firstName, string lastName, DateTime dateOfBirth,
                       string gender, string email, string phoneNumber, string address,
                       string position, DateTime joiningDate, DateTime? terminationDate)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Position = position;
            JoiningDate = joiningDate;
            TerminationDate = terminationDate;
        }

        public int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}