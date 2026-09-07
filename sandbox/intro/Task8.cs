using System;

public static class Task8
{
    public static void Run()
    {
        Console.WriteLine("Weight: ");
        double weight = double.Parse(Console.ReadLine()!);
        Console.WriteLine("Height: ");
        double height = double.Parse(Console.ReadLine()!);
        Console.WriteLine("Price: "); 
        double price = double.Parse(Console.ReadLine()!);
        Console.WriteLine("Visits: ");
        int visits = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Discount: ");
        int discount = int.Parse(Console.ReadLine()!);
        Console.WriteLine("BirthYear: ");
        int birthYear = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Systolic: ");
        int systolic = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Diastolic: ");
        int diastolic = int.Parse(Console.ReadLine()!);

        double imt = CalculateIMT(weight, height);
        Console.WriteLine($"IMT: {imt:F2} -> {GetIMTCategory(imt)}");

        double cost = CalculateCost(price, visits, discount);
        Console.WriteLine($"Amount: {cost:F2} grn");

        int age = 2026 - birthYear;
        Console.WriteLine($"Age: {age} y., Category: {GetAgeCategory(age)}");

        string pressureStatus = GetPressureStatus(systolic, diastolic);
        Console.WriteLine($"Pressure: {systolic}/{diastolic} — {pressureStatus}");
    }

    static double CalculateIMT(double weight, double height)
    {
        return weight / (height * height);
    }

    static string GetIMTCategory(double imt)
    {
        if (imt < 18.5) return "u need to eat";
        if (imt < 25) return "normal";
        if (imt < 30) return "below fatness";
        return "fatness";
    }

    static double CalculateCost(double price, int visits, int discount)
    {
        return price * visits * (1 - discount / 100.0);
    }

    static string GetAgeCategory(int age)
    {
        if (age <= 17) return "kid";
        if (age <= 59) return "adult";
        return "pensioner";
    }

    static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80) return "normal";
        if (systolic < 130 && diastolic < 80) return "over normal";
        if (systolic < 140 || diastolic < 90) return "gipertonia 1lvl";
        return "gipertonia 2lvl";
    }
}
