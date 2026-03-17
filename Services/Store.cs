using System;
using System.Collections.Generic;
using System.Linq;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services;

public class Store
{
    private readonly List<Product> _catalog = new();

    public Store()
    {
        // Seed with sample products
        _catalog.Add(new Product(1, "Laptop",         39999.00m, 10));
        _catalog.Add(new Product(2, "Mechanical KB",   2999.00m, 25));
        _catalog.Add(new Product(3, "Wireless Mouse",   899.00m, 40));
        _catalog.Add(new Product(4, "USB-C Hub",       1299.00m, 15));
        _catalog.Add(new Product(5, "Headset",         1999.00m, 20));
    }

    public IReadOnlyList<Product> ListProduct() => 
        _catalog.AsReadOnly();

    public Product? GetProductById(int id)  => 
        _catalog.FirstOrDefault(p => p.Id == id);

    public void ShowCatalog()
    {
        Console.WriteLine();
        Console.WriteLine("=== Product Catalog ===");
        foreach(var p in _catalog)
        {
            Console.WriteLine(p);
        }
    }
}