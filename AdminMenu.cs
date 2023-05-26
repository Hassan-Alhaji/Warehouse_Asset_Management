using System;
using System.Data.SqlClient;

public static class AdminMenu
{
    /// <summary>
    /// Displays the admin menu and handles user input for various admin operations.
    /// </summary>
    public static void ShowAdminMenu()
    {
        while (true)
        {
            Console.WriteLine("Admin Menu");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Update User");
            Console.WriteLine("3. Add Device");
            Console.WriteLine("4. Update Device");
            Console.WriteLine("5. Display All Devices");
            Console.WriteLine("6. Display Assigned Devices");
            Console.WriteLine("7. Assign Device to User");
            Console.WriteLine("8. Unassign Device from User");
            Console.WriteLine("9. Display Available Devices");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddUser();
                    break;
                case 2:
                    UpdateUser();
                    break;
                case 3:
                    AddDevice();
                    break;
                case 4:
                    UpdateDevice();
                    break;
                case 5:
                    DisplayAllDevices();
                    break;
                case 6:
                    DisplayAssignedDevices();
                    break;
                case 7:
                    AssignDeviceToUser();
                    break;
                case 8:
                    UnassignDeviceFromUser();
                    break;
                case 9:
                    DisplayAvailableDevices();
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

    /// <summary>
    /// Adds a user to the Users table.
    /// </summary>
    static void AddUser()
    {
        Console.WriteLine("Add User");

        Console.Write("Enter the user's name: ");
        string name = Console.ReadLine();

        Console.Write("Enter the user's phone number: ");
        int phone = Convert.ToInt32(Console.ReadLine());

        // Set IsAdmin to 0 (non-admin)
        int isAdmin = 0;

        // Code to add user to the Users table
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "INSERT INTO Users (name, phone, IsAdmin) VALUES (@name, @phone, @isAdmin)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@isAdmin", isAdmin);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("User added successfully!");

                    // Display all users
                    DisplayAllUsers();
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the user.");
                }
            }
        }
    }

    // ...

    /// <summary>
    /// Displays all users in the Users table.
    /// </summary>
    private static void DisplayAllUsers()
    {
        Console.WriteLine("All Users:");
        Console.WriteLine("\n ========================================");

        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "SELECT * FROM Users";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int phone = reader.GetInt32(2);
                        int isAdmin = reader.GetInt32(3);

                        Console.WriteLine($"User_ID: {userId}, Name: {name}, Phone: {phone}, IsAdmin: {(isAdmin == 1 ? "Yes" : "No")}");
                    }
                }
                Console.WriteLine("\n ========================================");
            }
        }
    }

    /// <summary>
    /// Updates a user's information in the Users table.
    /// </summary>
    static void UpdateUser()
    {
        Console.WriteLine("Update User \n");

        DisplayAllUsers();

        Console.Write("\nEnter the user ID to update: ");
        int userId = Convert.ToInt32(Console.ReadLine());

        // Check if the user is "admin"
        if (userId ==1)
        {
            Console.WriteLine("'admin' is not allowed to be updated.");
            return;
        }

        Console.Write("Enter the new name: ");
        string name = Console.ReadLine();

        Console.Write("Enter the new phone number: ");
        int phone = Convert.ToInt32(Console.ReadLine());

        // Code to update user in the Users table
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "UPDATE Users SET name = @name, phone = @phone WHERE ID = @userId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@userId", userId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("User updated successfully!");

                    // Display all users
                    DisplayAllUsers();
                }
                else
                {
                    Console.WriteLine("An error occurred while updating the user.");
                }
            }
        }
    }

    /// <summary>
    /// Adds a device to the Devices table.
    /// </summary>
    static void AddDevice()
    {
        Console.WriteLine("Add Device");

        Console.Write("Enter the device type (laptop, Phone ..): ");
        string name = Console.ReadLine();

        Console.Write("Enter the " + name + " serial number: ");
        string serialNumber = Console.ReadLine();

        // Code to add device to the Devices table
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "INSERT INTO Devices (Name, SerialNumber, IsAssigned) VALUES (@name, @serialNumber, 0)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@serialNumber", serialNumber);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine(name + " Device added successfully!\n");
                    DisplayAllDevices();
                }
                else
                {
                    Console.WriteLine("An error occurred while adding the device.");
                }
            }
        }
    }

    /// <summary>
    /// Displays all devices in the Devices table.
    /// </summary>
    static void DisplayAllDevices()
    {
        Console.WriteLine("Display All Devices");
        Console.WriteLine("\n ========================================");

        // Code to retrieve and display all devices from the Devices table
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "SELECT * FROM Devices";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("ID\tName\t\tSerial Number");

                    while (reader.Read())
                    {
                        int deviceId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string serialNumber = reader.GetString(2);

                        Console.WriteLine($"{deviceId}\t{name}\t{serialNumber}");
                    }
                }
            }
            Console.WriteLine("\n ========================================");
        }

    }

    /// <summary>
    /// Updates a device's information in the Devices table.
    /// </summary>
    static void UpdateDevice()
    {
        Console.WriteLine("Update Device \n");

        DisplayAllDevices();

        Console.Write("\nEnter the device ID to update: ");
        int deviceId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter the new name: ");
        string newName = Console.ReadLine();

        Console.Write("Enter the new serial number: ");
        string newSerialNumber = Console.ReadLine();

        // Code to update the device in the Devices table
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string updateQuery = "UPDATE Devices SET Name = @newName, SerialNumber = @newSerialNumber WHERE ID = @deviceId";
            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@newName", newName);
                updateCommand.Parameters.AddWithValue("@newSerialNumber", newSerialNumber);
                updateCommand.Parameters.AddWithValue("@deviceId", deviceId);

                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Device updated successfully!\n");

                    // Display all devices after updating
                    DisplayAllDevices();
                }
                else
                {
                    Console.WriteLine("No device found with the specified ID.");
                }
            }
        }
    }

    /// <summary>
    /// Displays all available devices (not currently assigned to a user).
    /// </summary>
    static void DisplayAvailableDevices()
    {
        Console.WriteLine("Available Devices:");
        Console.WriteLine("\n ========================================");

        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "SELECT * FROM Devices WHERE ID NOT IN (SELECT Device_id FROM Status)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int deviceId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string serialNumber = reader.GetString(2);

                        Console.WriteLine($"Device ID: {deviceId}, Name: {name}, Serial Number: {serialNumber}");
                    }
                }
                Console.WriteLine("\n ========================================");
            }
        }
    }


    /// <summary>
    /// Assigns a device to a user.
    /// </summary>
    static void DeleteDevice()
    {
        Console.WriteLine("Delete Device");
        // Code to delete device from the Devices table
        // ...
        Console.WriteLine("Device deleted successfully!");
    }

    static void AssignDeviceToUser()
    {
        Console.WriteLine("Assign Device to User");
        DisplayAllUsers();

        Console.Write("Enter the user ID: ");
        int userId = Convert.ToInt32(Console.ReadLine());
        DisplayAvailableDevices();

        Console.Write("Enter the device ID: ");
        int deviceId = Convert.ToInt32(Console.ReadLine());


        // Check if the device is already assigned
        if (IsDeviceAssigned(deviceId))
        {
            Console.WriteLine("Device is already assigned to a user.");
            return;
        }

        // Code to assign device to user
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string updateStatusQuery = @"INSERT INTO Status (Device_id, User_id, DateStart, IsActive)
                                    VALUES (@deviceId, @userId, GETDATE(), 1)";

            string updateDeviceQuery = @"UPDATE Devices
                                     SET IsAssigned = 1
                                     WHERE ID = @deviceId";

            using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, connection))
            using (SqlCommand updateDeviceCommand = new SqlCommand(updateDeviceQuery, connection))
            {
                updateStatusCommand.Parameters.AddWithValue("@deviceId", deviceId);
                updateStatusCommand.Parameters.AddWithValue("@userId", userId);
                updateDeviceCommand.Parameters.AddWithValue("@deviceId", deviceId);

                int statusRowsAffected = updateStatusCommand.ExecuteNonQuery();
                int deviceRowsAffected = updateDeviceCommand.ExecuteNonQuery();

                if (statusRowsAffected > 0 && deviceRowsAffected > 0)
                {
                    Console.WriteLine("Device assigned to user successfully!\n");
                    DisplayAssignedDevices();
                }
                else
                {
                    Console.WriteLine("An error occurred while assigning the device.");
                }
            }
        }
    }



    /// <summary>
    /// Checks if a device is already assigned to a user.
    /// </summary>
    /// The ID of the device to check.
    /// <returns>True if the device is assigned, false otherwise.</returns>
    static bool IsDeviceAssigned(int deviceId)
    {
        bool isAssigned = false;

        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Status WHERE Device_id = @deviceId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@deviceId", deviceId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    isAssigned = true;
                }
            }
        }

        return isAssigned;
    }

    /// <summary>
    /// Displays all devices currently assigned to users.
    /// </summary>
    static void DisplayAssignedDevices()
    {
        Console.WriteLine("Assigned Devices:\n");
        Console.WriteLine("\n ========================================");

        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string query = @"SELECT d.ID, d.Name, d.SerialNumber, u.Name AS UserName
                         FROM Devices d
                         JOIN Status s ON d.ID = s.Device_id
                         JOIN Users u ON s.User_id = u.ID
                         WHERE s.IsActive = 1";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Device ID\tName\t\tSerial Number\tAssigned To");

                    while (reader.Read())
                    {
                        int deviceId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string serialNumber = reader.GetString(2);
                        string userName = reader.GetString(3);

                        Console.WriteLine($"{deviceId}\t\t{name}\t{serialNumber}\t{userName}");
                    }
                }
                Console.WriteLine("\n ========================================");
            }
        }
    }


    /// <summary>
    /// Unassigns a device from a user.
    /// </summary>
    static void UnassignDeviceFromUser()
    {
        Console.WriteLine("Unassign Device from User");

        DisplayAssignedDevices();
        Console.Write("Enter the device ID: ");
        int deviceId = Convert.ToInt32(Console.ReadLine());

        // Check if the device is assigned
        if (!IsDeviceAssigned(deviceId))
        {
            Console.WriteLine("Device is not currently assigned to a user.");
            return;
        }

        // Code to unassign device from user
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string updateStatusQuery = @"DELETE FROM Status
                                     WHERE Device_id = @deviceId";

            string updateDeviceQuery = @"UPDATE Devices
                                     SET IsAssigned = 0
                                     WHERE ID = @deviceId";

            using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, connection))
            using (SqlCommand updateDeviceCommand = new SqlCommand(updateDeviceQuery, connection))
            {
                updateStatusCommand.Parameters.AddWithValue("@deviceId", deviceId);
                updateDeviceCommand.Parameters.AddWithValue("@deviceId", deviceId);

                int statusRowsAffected = updateStatusCommand.ExecuteNonQuery();
                int deviceRowsAffected = updateDeviceCommand.ExecuteNonQuery();

                if (statusRowsAffected > 0 && deviceRowsAffected > 0)
                {
                    Console.WriteLine("Device unassigned from user successfully!");

                    // Make the device available for the next assignee
                    UpdateDeviceIsAssigned(deviceId, false);
                }
                else
                {
                    Console.WriteLine("An error occurred while unassigning the device.");
                }
            }
        }
    }

    /// <summary>
    /// Updates the IsAssigned status of a device in the Devices table.
    /// </summary>
    /// deviceId The ID of the device to update.
    /// isAssigned he new IsAssigned status value.</param>
    static void UpdateDeviceIsAssigned(int deviceId, bool isAssigned)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString.WHDBConnection))
        {
            connection.Open();

            string updateQuery = @"UPDATE Devices
                               SET IsAssigned = @isAssigned
                               WHERE ID = @deviceId";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@isAssigned", isAssigned ? 1 : 0);
                command.Parameters.AddWithValue("@deviceId", deviceId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Device List Updated\n");
                    DisplayAvailableDevices();
                }
                else
                {
                    Console.WriteLine("An error occurred while updating the device IsAssigned status.");
                }
            }
        }
    }

}
