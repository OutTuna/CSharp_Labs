using System;

public static class Task3
{
    public static void Run()
    {
        Console.WriteLine("BirthYear: ");
        int birthYear = int.Parse(Console.ReadLine()!);
        int age = 2026 - birthYear;

        string category;

        if (age <= 17)
        {
            category = "Kid";
        }
        else if (age <= 59)
        {
            category = "Adult";
        }
        else
        {
            category = "Pensioner";
        }

        Console.WriteLine($"Age: {age} y.");
        Console.WriteLine($"Category: {category}");
    }
}