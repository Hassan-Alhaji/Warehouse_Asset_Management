using System.Data.SqlClient;
using System;

public static class UserMenu
{
    private static string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=WHDB;Integrated Security=True;";

    /// <summary>
    /// Displays the user menu and handles user input.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    public static void ShowUserMenu(string username)
    {
        int userId = GetUserIdByUsername(username);

        if (userId != -1)
        {
            while (true)
            {
                Console.WriteLine("User Menu");
                Console.WriteLine("1. Display Available Devices"); // Option to display available devices
                Console.WriteLine("2. Display All Devices Under your Name");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        DisplayAvailableDevices();
                        break;
                    case 2:
                        DisplayAllDevicesForUser(userId);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Invalid username. User not found.");
        }
    }

    /// <summary>
    /// Retrieves the user ID based on the username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user ID if found, or -1 if not found.</returns>
    private static int GetUserIdByUsername(string username)
    {
        int userId = -1;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT ID FROM Users WHERE name = @username";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    userId = reader.GetInt32(0);
                }

                reader.Close();
            }
        }

        return userId;
    }

    /// <summary>
    /// Displays the available devices that are not assigned to any user.
    /// </summary>
    private static void DisplayAvailableDevices()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT ID, Name FROM Devices WHERE IsAssigned = 0";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("Available Devices:");
                while (reader.Read())
                {
                    int deviceId = reader.GetInt32(0);
                    string deviceName = reader.GetString(1);
                    Console.WriteLine($"Device ID: {deviceId}, Device Name: {deviceName}");
                }

                reader.Close();
            }
        }
    }

    /// <summary>
    /// Displays all devices assigned to a user based on the user ID.
    /// </summary>

    private static void DisplayAllDevicesForUser(int userId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT D.Name FROM Devices D INNER JOIN Status S ON D.ID = S.Device_id WHERE S.User_id = @userId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("Devices assigned to user:");
                while (reader.Read())
                {
                    string deviceName = reader.GetString(0);
                    Console.WriteLine($"Device: {deviceName}");
                }

                reader.Close();
            }
        }
    }
}
