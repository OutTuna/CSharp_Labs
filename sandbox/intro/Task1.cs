using System;

public static class Task1
{
    public static void Run()
    {
        Console.WriteLine("Enter weight: ");
        double weight = double.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter height: "); 
        double height = double.Parse(Console.ReadLine()!);

        double IMT = weight / (height * height);
        
        Console.WriteLine($"IMT: {IMT:F2}");
    }
}