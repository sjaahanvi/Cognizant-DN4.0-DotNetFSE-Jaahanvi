using System;
using System.Collections.Generic;

public class Product
{
    public int ProductId;
    public string ProductName;
    public int Quantity;
    public double Price;

    public Product(int id, string name, int qty, double price)
    {
        ProductId = id;
        ProductName = name;
        Quantity = qty;
        Price = price;
    }

    public void Display()
    {
        Console.WriteLine($"{ProductId} - {ProductName} - Qty: {Quantity}, Price: {Price:C}");
    }
}

class InventorySystem
{
    Dictionary<int, Product> inventory = new Dictionary<int, Product>();

    public void AddProduct(Product p)
    {
        if (!inventory.ContainsKey(p.ProductId))
        {
            inventory[p.ProductId] = p;
            Console.WriteLine("Product added.");
        }
        else Console.WriteLine("Product ID already exists!");
    }

    public void UpdateProduct(int id, int qty, double price)
    {
        if (inventory.ContainsKey(id))
        {
            inventory[id].Quantity = qty;
            inventory[id].Price = price;
            Console.WriteLine("Product updated.");
        }
        else Console.WriteLine("Product not found.");
    }

    public void DeleteProduct(int id)
    {
        if (inventory.Remove(id))
            Console.WriteLine("Product deleted.");
        else
            Console.WriteLine("Product not found.");
    }

    public void DisplayInventory()
    {
        Console.WriteLine("\nInventory:");
        foreach (var product in inventory.Values)
            product.Display();
    }
}

class Program
{
    static void Main()
    {
        InventorySystem system = new InventorySystem();

        system.AddProduct(new Product(101, "Mouse", 50, 299.99));
        system.AddProduct(new Product(102, "Keyboard", 30, 499.99));
        system.DisplayInventory();

        system.UpdateProduct(101, 60, 279.99);
        system.DeleteProduct(102);

        system.DisplayInventory();
    }
}
