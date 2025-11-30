using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonManagementSystem.Models;
using PersonManagementSystem.Utils;

namespace PersonManagementSystem.Services
{
    // This service manages all operations related to people
    public class PersonService
    {
        private List<Person> people;
        private string filePath = Path.Combine("Data", "People.txt");
        private Logger logger;
        private Validator validator;
        private string currentUser;

        // Constructor
        public PersonService(string username, Logger loggerInstance)
        {
            people = new List<Person>();
            currentUser = username;
            logger = loggerInstance;
            validator = new Validator();
            LoadPeople();
        }

        // Load people from file
        private void LoadPeople()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        // Split by pipe character: id|firstname|lastname|phone|city|balance
                        string[] parts = line.Split('|');

                        if (parts.Length == 6)
                        {
                            int id = int.Parse(parts[0]);
                            string firstName = parts[1];
                            string lastName = parts[2];
                            string phone = parts[3];
                            string city = parts[4];
                            decimal balance = decimal.Parse(parts[5]);

                            people.Add(new Person(id, firstName, lastName, phone, city, balance));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading people: {ex.Message}");
            }
        }

        // Save all people to file
        public void SavePeople()
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (Person person in people)
                {
                    lines.Add(person.ToFileFormat());
                }

                File.WriteAllLines(filePath, lines);
                Console.WriteLine("\nChanges saved successfully!");
                logger.Log(currentUser, "Saved changes to file");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving people: {ex.Message}");
            }
        }

        // Display all people
        public void ShowPeople()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          PEOPLE LIST");
            Console.WriteLine("========================================");
            Console.WriteLine();

            if (people.Count == 0)
            {
                Console.WriteLine("No people found in the system.");
            }
            else
            {
                foreach (Person person in people)
                {
                    person.Display();
                }
            }

            logger.Log(currentUser, "Viewed people list");
        }

        // Add a new person with validations
        public void AddPerson()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          ADD NEW PERSON");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // Validate ID
            int id;
            while (true)
            {
                Console.Write("Enter ID: ");
                string idText = Console.ReadLine();

                if (!validator.IsValidId(idText, out id))
                {
                    Console.WriteLine("Error: ID must be a positive number. Try again.");
                    continue;
                }

                // Check if ID already exists
                if (people.Any(p => p.Id == id))
                {
                    Console.WriteLine("Error: A person with this ID already exists. Try again.");
                    continue;
                }

                break; // ID is valid and unique
            }

            // Validate First Name
            string firstName;
            while (true)
            {
                Console.Write("Enter First Name: ");
                firstName = Console.ReadLine();

                if (!validator.IsNotEmpty(firstName))
                {
                    Console.WriteLine("Error: First name is required. Try again.");
                    continue;
                }

                break; // Valid first name
            }

            // Validate Last Name
            string lastName;
            while (true)
            {
                Console.Write("Enter Last Name: ");
                lastName = Console.ReadLine();

                if (!validator.IsNotEmpty(lastName))
                {
                    Console.WriteLine("Error: Last name is required. Try again.");
                    continue;
                }

                break; // Valid last name
            }

            // Validate Phone
            string phone;
            while (true)
            {
                Console.Write("Enter Phone: ");
                phone = Console.ReadLine();

                if (!validator.IsValidPhone(phone))
                {
                    Console.WriteLine("Error: Phone must contain at least 7 characters with numbers. Try again.");
                    continue;
                }

                break; // Valid phone
            }

            // Validate City
            string city;
            while (true)
            {
                Console.Write("Enter City: ");
                city = Console.ReadLine();

                if (!validator.IsNotEmpty(city))
                {
                    Console.WriteLine("Error: City is required. Try again.");
                    continue;
                }

                break; // Valid city
            }

            // Validate Balance
            decimal balance;
            while (true)
            {
                Console.Write("Enter Balance: ");
                string balanceText = Console.ReadLine();

                if (!validator.IsValidBalance(balanceText, out balance))
                {
                    Console.WriteLine("Error: Balance must be a positive number. Try again.");
                    continue;
                }

                break; // Valid balance
            }

            // Create and add the person
            Person newPerson = new Person(id, firstName, lastName, phone, city, balance);
            people.Add(newPerson);

            Console.WriteLine("\nPerson added successfully!");
            logger.Log(currentUser, $"Added new person: {firstName} {lastName} (ID: {id})");
        }

        // Edit an existing person
        public void EditPerson()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          EDIT PERSON");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.Write("Enter the ID of the person to edit: ");
            string idText = Console.ReadLine();

            int id;
            if (!validator.IsValidId(idText, out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            // Find the person
            Person person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                Console.WriteLine($"No person found with ID {id}.");
                return;
            }

            Console.WriteLine("\nCurrent data:");
            person.Display();

            Console.WriteLine("Enter new data (press ENTER to keep current value):");
            Console.WriteLine();

            // Edit First Name
            Console.Write($"First Name [{person.FirstName}]: ");
            string newFirstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFirstName))
            {
                person.FirstName = newFirstName;
            }

            // Edit Last Name
            Console.Write($"Last Name [{person.LastName}]: ");
            string newLastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLastName))
            {
                person.LastName = newLastName;
            }

            // Edit Phone
            Console.Write($"Phone [{person.Phone}]: ");
            string newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone))
            {
                if (validator.IsValidPhone(newPhone))
                {
                    person.Phone = newPhone;
                }
                else
                {
                    Console.WriteLine("Invalid phone format. Keeping previous value.");
                }
            }

            // Edit City
            Console.Write($"City [{person.City}]: ");
            string newCity = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCity))
            {
                person.City = newCity;
            }

            // Edit Balance
            Console.Write($"Balance [{person.Balance}]: ");
            string newBalanceText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newBalanceText))
            {
                decimal newBalance;
                if (validator.IsValidBalance(newBalanceText, out newBalance))
                {
                    person.Balance = newBalance;
                }
                else
                {
                    Console.WriteLine("Invalid balance format. Keeping previous value.");
                }
            }

            Console.WriteLine("\nPerson updated successfully!");
            logger.Log(currentUser, $"Edited person with ID: {id}");
        }

        // Delete a person
        public void DeletePerson()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("          DELETE PERSON");
            Console.WriteLine("========================================");
            Console.WriteLine();

            Console.Write("Enter the ID of the person to delete: ");
            string idText = Console.ReadLine();

            int id;
            if (!validator.IsValidId(idText, out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            // Find the person
            Person person = people.FirstOrDefault(p => p.Id == id);

            if (person == null)
            {
                Console.WriteLine($"No person found with ID {id}.");
                return;
            }

            // Show person data
            Console.WriteLine("\nPerson to delete:");
            person.Display();

            // Ask for confirmation
            Console.Write("Are you sure you want to delete this person? (Y/N): ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation == "Y" || confirmation == "YES")
            {
                people.Remove(person);
                Console.WriteLine("\nPerson deleted successfully!");
                logger.Log(currentUser, $"Deleted person: {person.FirstName} {person.LastName} (ID: {id})");
            }
            else
            {
                Console.WriteLine("\nDeletion cancelled.");
            }
        }

        // Generate report with subtotals by city
        public void GenerateReport()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("       REPORT BY CITY");
            Console.WriteLine("========================================");
            Console.WriteLine();

            if (people.Count == 0)
            {
                Console.WriteLine("No people in the system.");
                return;
            }

            // Group people by city
            var groupedByCity = people.GroupBy(p => p.City).OrderBy(g => g.Key);

            decimal totalGeneral = 0;

            foreach (var cityGroup in groupedByCity)
            {
                Console.WriteLine($"Ciudad: {cityGroup.Key}");
                Console.WriteLine();
                Console.WriteLine("ID\tNombres\t\tApellidos\tSaldo");
                Console.WriteLine("--\t--------------\t------------\t----------");

                decimal subtotal = 0;

                foreach (var person in cityGroup)
                {
                    Console.WriteLine($"{person.Id}\t{person.FirstName}\t\t{person.LastName}\t\t{person.Balance:N2}");
                    subtotal += person.Balance;
                }

                Console.WriteLine("\t\t\t\t\t========");
                Console.WriteLine($"Total: {cityGroup.Key}\t\t\t\t{subtotal:N2}");
                Console.WriteLine();

                totalGeneral += subtotal;
            }

            Console.WriteLine("\t\t\t\t\t========");
            Console.WriteLine($"Total General:\t\t\t\t{totalGeneral:N2}");
            Console.WriteLine();

            logger.Log(currentUser, "Generated report by city");
        }
    }
}