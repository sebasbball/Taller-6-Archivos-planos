namespace PersonManagementSystem.Models
{
    // This class represents a user who can login to the system
    public class User
    {
        // Username for login
        public string Username { get; set; }

        // Password for authentication
        public string Password { get; set; }

        // Indicates if the user account is active or blocked
        public bool IsActive { get; set; }

        // Constructor to create a user
        public User(string username, string password, bool isActive)
        {
            Username = username;
            Password = password;
            IsActive = isActive;
        }

        // Method to convert user data to file format
        public string ToFileFormat()
        {
            return $"{Username},{Password},{IsActive.ToString().ToLower()}";
        }
    }
}