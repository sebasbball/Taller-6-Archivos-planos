using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonManagementSystem.Models;

namespace PersonManagementSystem.Services
{
    // This service handles user authentication and login
    public class AuthService
    {
        private string usersFilePath = Path.Combine("Data", "Users.txt");
        private List<User> users;

        // Constructor loads users from file
        public AuthService()
        {
            LoadUsers();
        }

        // Load all users from the Users.txt file
        private void LoadUsers()
        {
            users = new List<User>();

            try
            {
                if (File.Exists(usersFilePath))
                {
                    string[] lines = File.ReadAllLines(usersFilePath);

                    foreach (string line in lines)
                    {
                        // Split the line: username,password,isActive
                        string[] parts = line.Split(',');

                        if (parts.Length == 3)
                        {
                            string username = parts[0];
                            string password = parts[1];
                            bool isActive = bool.Parse(parts[2]);

                            users.Add(new User(username, password, isActive));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Warning: Users.txt file not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }

        // Try to login with username and password
        // Returns the username if successful, null if failed
        public string Login()
        {
            int attempts = 0;
            int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        LOGIN TO THE SYSTEM");
                Console.WriteLine("========================================");
                Console.WriteLine();

                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                // Find the user in our list
                User user = users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    // User doesn't exist
                    attempts++;
                    Console.WriteLine($"\nInvalid username or password. Attempts remaining: {maxAttempts - attempts}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
                else if (!user.IsActive)
                {
                    // User is blocked
                    Console.WriteLine("\nYour account is blocked. Contact the administrator.");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return null;
                }
                else if (user.Password != password)
                {
                    // Wrong password
                    attempts++;

                    if (attempts >= maxAttempts)
                    {
                        // Block the user after 3 failed attempts
                        user.IsActive = false;
                        SaveUsers();
                        Console.WriteLine("\nToo many failed attempts. Your account has been blocked.");
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        return null;
                    }

                    Console.WriteLine($"\nInvalid username or password. Attempts remaining: {maxAttempts - attempts}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
                else
                {
                    // Login successful!
                    Console.WriteLine($"\nWelcome, {username}!");
                    System.Threading.Thread.Sleep(1000); // Wait 1 second
                    return username;
                }
            }

            return null;
        }

        // Save all users back to the file (used when blocking a user)
        private void SaveUsers()
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (User user in users)
                {
                    lines.Add(user.ToFileFormat());
                }

                File.WriteAllLines(usersFilePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }
}