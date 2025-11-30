using System;

namespace PersonManagementSystem.Models
{
    // This class represents a person with their basic information
    public class Person
    {
        // Unique identifier for each person
        public int Id { get; set; }

        // First name of the person
        public string FirstName { get; set; }

        // Last name of the person
        public string LastName { get; set; }

        // Phone number
        public string Phone { get; set; }

        // City where the person lives
        public string City { get; set; }

        // Account balance
        public decimal Balance { get; set; }

        // Constructor to create a new person with all data
        public Person(int id, string firstName, string lastName, string phone, string city, decimal balance)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            City = city;
            Balance = balance;
        }

        // Empty constructor (needed sometimes)
        public Person()
        { }

        // Method to display person info in console
        public void Display()
        {
            Console.WriteLine($"{Id}\t\t{FirstName} {LastName}");
            Console.WriteLine($"\t\tPhone: {Phone}");
            Console.WriteLine($"\t\tCity: {City}");
            Console.WriteLine($"\t\tBalance:\t\t${Balance:N2}");
            Console.WriteLine();
        }

        // Method to convert person data to text format for saving
        public string ToFileFormat()
        {
            return $"{Id}|{FirstName}|{LastName}|{Phone}|{City}|{Balance}";
        }
    }
}

// Datos para validaciones:
// Username: jzuluaga - Password: P@ssw0rd123!
// Username: mbedoya - Password: S0yS3gur02025*