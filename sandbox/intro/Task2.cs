using System;

public static class Task2
{
    public static void Run()
    {
        Console.WriteLine("Price: ");
        double price = double.Parse(Console.ReadLine()!);
        Console.WriteLine("Visits: ");
        int visits = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Discount: ");
        int discount = int.Parse(Console.ReadLine()!);

        double total = price * visits * (1 - discount / 100.0);

        Console.WriteLine($"Total: {total:F2} grn.");
    }
}