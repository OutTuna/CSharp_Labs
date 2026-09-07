using System;

public static class Task7
{
    public static void Run()
    {
        Console.WriteLine("n: ");
        int n = int.Parse(Console.ReadLine()!);

        decimal[] costs = new decimal[n];
        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(Console.ReadLine()!);
        }

        // рахуємо суму, мінімум, та максимуму
        decimal sum = 0;
        decimal min = costs[0];
        decimal max = costs[0];
        foreach (decimal cost in costs)
        {
            sum += cost;
            if (cost < min) min = cost;
            if (cost > max) max = cost;
        }
        decimal average = sum / n;

        // рахуємо скільки вище середньго, використовуємо дані першого лупу.
        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if (costs[i] > average)
            {
                aboveAverage++;
            }
        }

        // перший котрий > 1000
        string firstOver1000 = "none";
        int index = 0;
        while (index < n)
        {
            if (costs[index] > 1000)
            {
                firstOver1000 = $"#{index + 1} — {costs[index]:F2} grn";
                break;
            }
            index++;
        }
        
        Console.WriteLine($"Count/Amount: {n}");
        Console.WriteLine($"Overall: {sum:F2} grn");
        Console.WriteLine($"Average: {average:F2} grn");
        Console.WriteLine($"Min / Max: {min:F2} / {max:F2} grn");
        Console.WriteLine($"Above average: {aboveAverage} from {n}");
        Console.WriteLine($"First > 1000: {firstOver1000}");
    }
}