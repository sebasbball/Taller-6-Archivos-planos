using System;
using System.Linq;

namespace PersonManagementSystem.Utils
{
    // This class contains methods to validate user input
    public class Validator
    {
        // Check if a string is not empty
        public bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        // Check if ID is a valid positive number
        public bool IsValidId(string idText, out int id)
        {
            if (int.TryParse(idText, out id))
            {
                return id > 0; // ID must be positive
            }
            return false;
        }

        // Check if phone number has a valid format (basic check)
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Phone should have at least 7 characters and contain numbers
            return phone.Length >= 7 && phone.Any(char.IsDigit);
        }

        // Check if balance is a valid positive number
        public bool IsValidBalance(string balanceText, out decimal balance)
        {
            if (decimal.TryParse(balanceText, out balance))
            {
                return balance >= 0; // Balance must be zero or positive
            }
            return false;
        }
    }
}