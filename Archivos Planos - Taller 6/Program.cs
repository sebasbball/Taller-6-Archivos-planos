using PersonManagementSystem.Models;
using PersonManagementSystem.Services;
using PersonManagementSystem.Utils;
using System;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonManagementSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create logger instance
            Logger logger = new Logger();

            // Create authentication service
            AuthService authService = new AuthService();

            // Try to login
            string username = authService.Login();

            // If login failed, exit the program
            if (username == null)
            {
                return;
            }

            // Log successful login
            logger.Log(username, "Logged in successfully");

            // Create person service with the logged user
            PersonService personService = new PersonService(username, logger);

            // Main menu loop
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"  Welcome, {username}!");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Show content");
                Console.WriteLine("2. Add person");
                Console.WriteLine("3. Edit person");
                Console.WriteLine("4. Delete person");
                Console.WriteLine("5. Generate report by city");
                Console.WriteLine("6. Save changes");
                Console.WriteLine("0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Choose an option: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        personService.ShowPeople();
                        break;

                    case "2":
                        personService.AddPerson();
                        break;

                    case "3":
                        personService.EditPerson();
                        break;

                    case "4":
                        personService.DeletePerson();
                        break;

                    case "5":
                        personService.GenerateReport();
                        break;

                    case "6":
                        personService.SavePeople();
                        break;

                    case "0":
                        Console.WriteLine("\nExiting the system...");
                        logger.Log(username, "Logged out");
                        exit = true;
                        continue;

                    default:
                        Console.WriteLine("\nInvalid option. Try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}