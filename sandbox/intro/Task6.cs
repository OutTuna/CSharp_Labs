using System;

public static class Task6
{
    public static void Run()
    {
        int card = int.Parse(Console.ReadLine()!);

        int lastDigit = card % 10;
        string department = lastDigit switch
        {
            0 or 1 => "overall therapy",
            2 or 3 => "hirurgiya",
            4 or 5 => "cardiologia",
            6 or 7 => "nevrologia",
            8 or 9 => "oftalmogia",
            _ => "unknown"
        };

        string privileged = card % 2 == 0 ? "yes" : "no";
        string checkup = card % 3 == 0 ? "yes" : "no";

        Console.WriteLine($"Department: {department}");
        Console.WriteLine($"Privileged: {privileged}");
        Console.WriteLine($"CheckUp: {checkup}");
    }
}