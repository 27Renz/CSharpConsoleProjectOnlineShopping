namespace OnlineShoppingSystem.Models;

public class Product
{   
    public int Id { get; set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityAvailable { get; private set; }

    public Product(int id, string name, decimal price, int quantityAvailable)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
        
        if(price < 0)
            throw new ArgumentOutOfRangeException(nameof(price));

        if(quantityAvailable < 0)
            throw new ArgumentOutOfRangeException(nameof(quantityAvailable));

        Id = id;
        Name = name;
        Price = price;
        QuantityAvailable = quantityAvailable;
    }
    public bool Order(int quantity)
    {
        if(quantity <= 0)
            return false;

        if(quantity > QuantityAvailable)
            return false;

        QuantityAvailable -= quantity;

        return true;
    }

    public void ChangePrice(decimal newPrice)
    {
        if(newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), "Price cannot be negative.");
        
        Price = newPrice;
    }

    public override string ToString()
    {
        return $"#{Id}  {Name,-20}  ₱{Price,8:0.00}  Stock: {QuantityAvailable}";
    }

}