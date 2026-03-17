using System;
using System.Security.Cryptography.X509Certificates;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Services;

namespace OnlineShoppingSystem;

internal class Program
{
    private const string ADMINPASSWORD = "admin123";

    static void Main()
    {
        var store = new Store();
        Customer? currentCustomer = null;

        while(true)
        {
            Console.WriteLine();
            Console.WriteLine("===== Online Shopping System (Console) =====");
            Console.WriteLine("1) Login");
            Console.WriteLine("2) View Catalog");
            Console.WriteLine("3) Add to Cart");
            Console.WriteLine("4) Remove from Cart");
            Console.WriteLine("5) View Cart");
            Console.WriteLine("6) Checkout");
            Console.WriteLine("7) Logout");
            Console.WriteLine("8) Admin: Change Product Price");
            Console.WriteLine("0) Exit");
            Console.Write("Choose: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    currentCustomer = HandleLogin(currentCustomer);
                    break;
                case "2":
                    store.ShowCatalog();
                    break;
                case "3":
                    RequireLogin(currentCustomer, () =>
                    {
                        store.ShowCatalog();
                        int id = ReadInt("Enter Product Id to add: ");
                        var product = store.GetProductById(id);
                        if (product == null)
                        {
                            Console.WriteLine("Product not found.");
                            return;
                        }
                        int qty = ReadInt("Enter quantity: ");
                        var selected = currentCustomer!.SelectProduct(product);
                        if (selected != null)
                        {
                            currentCustomer.Cart.Add(selected, qty);
                            Console.WriteLine($"Added {qty} x {selected.Name} to cart.");
                        }
                    });
                    break;
                case "4":
                    RequireLogin(currentCustomer, () =>
                    {
                        currentCustomer!.Cart.View();
                        int id = ReadInt("Enter Product Id to remove: ");
                        bool removed = currentCustomer.Cart.Remove(id);
                        Console.WriteLine(removed ? "Removed." : "Item not found in cart.");
                    });
                    break;
                case "5":
                    if (currentCustomer != null) currentCustomer.Cart.View();
                    else Console.WriteLine("Please login first.");
                    break;
                case "6":
                    RequireLogin(currentCustomer, () =>
                    {
                        var (ok, msg) = currentCustomer!.Cart.Checkout();
                        Console.WriteLine(msg);
                    });
                    break;
                case "7":
                    if (currentCustomer != null)
                    {
                        currentCustomer.Logout();
                        currentCustomer = null;
                    }
                    else Console.WriteLine("No user is logged in.");
                    break;
                case "8":
                    Console.Write("Enter admin password: ");
                    var pwd = Console.ReadLine() ?? "";
                    if (pwd != ADMINPASSWORD)
                    {
                        Console.WriteLine("Invalid admin password.");
                        break;
                    }
                    store.ShowCatalog();
                    int pid = ReadInt("Enter Product Id to change price: ");
                    var prod = store.GetProductById(pid);
                    if (prod == null)
                    {
                        Console.WriteLine("Product not found.");
                        break;
                    }
                    decimal newPrice = ReadDecimal("Enter new price: ");
                    try
                    {
                        prod.ChangePrice(newPrice);
                        Console.WriteLine($"Price updated: {prod.Name} -> ₱{prod.Price:0.00}");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
    static Customer HandleLogin(Customer? current)
    {
        if (current != null && current.IsLoggedIn)
        {
            Console.WriteLine($"Already logged in as {current.Name}.");
            return current;
        }
        Console.Write("Enter your name: ");
        var name = Console.ReadLine() ?? "Guest";
        Console.Write("Enter your address: ");
        var address = Console.ReadLine() ?? "N/A";
        var customer = new Customer(name, address);
        customer.Login();
        return customer;
    }

    static void RequireLogin(Customer? customer, Action action)
    {
        if (customer == null || !customer.IsLoggedIn)
        {
            Console.WriteLine("Please login first.");
            return;
        }
        action();
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= 0)
                return value;
            Console.WriteLine("Please enter a valid non-negative integer.");
        }
    }

    static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                return value;
            Console.WriteLine("Please enter a valid non-negative amount.");
        }
    }
}