using System;

public class Product
{
    public int ProductId;
    public string ProductName;
    public string Category;

    public Product(int id, string name, string category)
    {
        ProductId = id;
        ProductName = name;
        Category = category;
    }

    public void Display()
    {
        Console.WriteLine($"{ProductId} - {ProductName} - {Category}");
    }
}

class Program
{
    public static int LinearSearch(Product[] products, int targetId)
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].ProductId == targetId)
                return i;
        }
        return -1;
    }

    public static int BinarySearch(Product[] products, int targetId)
    {
        int left = 0, right = products.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (products[mid].ProductId == targetId)
                return mid;
            else if (products[mid].ProductId < targetId)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return -1;
    }

    static void Main(string[] args)
    {
        Product[] products = new Product[]
        {
            new Product(103, "Shoes", "Footwear"),
            new Product(101, "Phone", "Electronics"),
            new Product(105, "Laptop", "Electronics"),
            new Product(102, "T-shirt", "Clothing"),
            new Product(104, "Watch", "Accessories")
        };

        Console.WriteLine("Linear Search: Searching for Product ID 102");
        int linearIndex = LinearSearch(products, 102);
        if (linearIndex != -1)
            products[linearIndex].Display();
        else
            Console.WriteLine("Product not found.");

        Array.Sort(products, (a, b) => a.ProductId.CompareTo(b.ProductId));

        Console.WriteLine("\nBinary Search: Searching for Product ID 102");
        int binaryIndex = BinarySearch(products, 102);
        if (binaryIndex != -1)
            products[binaryIndex].Display();
        else
            Console.WriteLine("Product not found.");
    }
}
