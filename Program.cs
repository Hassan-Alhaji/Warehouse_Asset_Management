using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the warehouse asset management program!");
        Console.WriteLine("Please login:");

        string username = Console.ReadLine();

        if (username == "admin")
        {
            AdminMenu.ShowAdminMenu();
        }
        else
        {
            UserMenu.ShowUserMenu(username);
        }

        Console.WriteLine("Thank you for using the warehouse asset management program. Press any key to exit.");
        Console.ReadKey();
    }
}
