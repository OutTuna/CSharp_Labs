using System;
static class Task1
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);

        double[] weights = new double[n];
        for (int i = 0; i < n; i++)
        {
            weights[i] = double.Parse(Console.ReadLine()!);
        }

        double sum = 0;
        double min = weights[0];
        double max = weights[0];

        foreach (double w in weights)
        {
            sum += w;
            if (w < min) min = w;
            if (w > max) max = w;
        }

        double avg = sum / n;
        
        int aboveAvg = 0;
        for (int i = 0; i < n; i++)
        {
            if (weights[i] > avg) aboveAvg++;
        }

        Console.WriteLine($"Кількість: {n}");
        Console.WriteLine($"Середня вага: {avg:F1} кг");
        Console.WriteLine($"Мін / Макс: {min:F1} / {max:F1} кг");
        Console.WriteLine($"Вище середнього: {aboveAvg} з {n}");
    }
}