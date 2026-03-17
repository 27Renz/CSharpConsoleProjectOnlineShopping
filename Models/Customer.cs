namespace OnlineShoppingSystem.Models;

public class Customer
{
    public string Name { get; set; }

    public string Address { get; set; }
    public bool IsLoggedIn { get; private set; }
    public ShoppingCart Cart { get; } = new();

    public Customer(string name, string address)
    {
        Name = name;
        Address = address;
        IsLoggedIn = false;
    }

    public void Login()
    {
        if(IsLoggedIn) return;
        IsLoggedIn = true;
        Console.WriteLine($"Welcome, {Name}!");
    }

    public void Logout()
    {
        if(!IsLoggedIn) return;
        IsLoggedIn = false;
        Console.WriteLine("You have been logged out.");
    }

    public Product? SelectProduct(Product product)
    {
        if(!IsLoggedIn)
        {
            Console.WriteLine("Please login first.");
            return null;
        }

        return product;
    }
}