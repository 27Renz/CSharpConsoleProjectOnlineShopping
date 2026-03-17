namespace OnlineShoppingSystem.Models;

public class CartItem
{
    public Product Product { get; }
    public int Quantity { get; private set; }

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity > 0 ? quantity : 1;
    }

    public void Increase (int by) => Quantity += by;
    public void SetQuantity(int newQty)
    {
        if(newQty <= 0)
            newQty = 1;
        
        Quantity = newQty;
    }

    public decimal LineTotal => Product.Price * Quantity;

    public override string ToString() =>
        $"{Product.Name} x {Quantity} = ₱{LineTotal:0.00}";

}