using System;
using System.IO;

namespace PersonManagementSystem.Utils
{
    // This class handles logging all operations to a file
    public class Logger
    {
        private string logFilePath = "log.txt";

        // Method to write a log entry with username and action
        public void Log(string username, string action)
        {
            try
            {
                // Create the log message with timestamp
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] User: {username} | Action: {action}";

                // Append the log entry to the file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // If something goes wrong, show error but don't crash
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }
    }
}