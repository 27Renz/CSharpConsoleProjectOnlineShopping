using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShoppingSystem.Models;
public class ShoppingCart
{
    private readonly List<CartItem> _items = new();
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public void Add(Product product, int quantity)
    {
        if(product == null)
            throw new ArgumentNullException(nameof(product));
        
        if(quantity <= 0)
            quantity = 1;

        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if(existing != null)
        {
            existing.Increase(quantity);
        }
        else
        {
            _items.Add(new CartItem(product, quantity));
        }
    }

    public bool Remove(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if(item == null)
            return false;

        _items.Remove(item);
        return true;
    }

    public void Clear() => _items.Clear();
    public decimal Total => _items.Sum(i => i.LineTotal);
    
    public void View()
    {
        if(_items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        Console.WriteLine("=== Your Cart ===");

        foreach(var item in _items)
        {
            Console.WriteLine($"- {item}");
        }
        Console.WriteLine($"Total: ₱{Total:0.00}");
    }

    
    public (bool ok, string message) Checkout()
        {
            if (_items.Count == 0) 
                return (false, "Cart is empty.");

            // Check stock first
            foreach (var item in _items)
            {
                if (item.Quantity > item.Product.QuantityAvailable)
                {
                    return (false, $"Insufficient stock for {item.Product.Name}. Available: {item.Product.QuantityAvailable}");
                }
            }

            // Deduct stock
            foreach (var item in _items)
            {
                item.Product.Order(item.Quantity);
            }

            var grandTotal = Total;
            _items.Clear();
            return (true, $"Checkout successful! You paid ₱{grandTotal:0.00}");
        }
}