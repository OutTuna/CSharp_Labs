using System;

public class Task6
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);

        int[][] costs = new int[n][];
        for (int i = 0; i < n; i++)
        {
            int k = int.Parse(Console.ReadLine()!);
            costs[i] = new int[k];

            for (int j = 0; j < k; j++)
            {
                costs[i][j] = int.Parse(Console.ReadLine()!);
            }
        }

        int[] totals = new int[n];
        int bestIdx = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < costs[i].Length; j++)
            {
                totals[i] += costs[i][j];
            }
            if (totals[i] > totals[bestIdx]) bestIdx = i;
        }

        for (int i = 0; i < n; i++)
        {
            double avg = (double)totals[i] / costs[i].Length;
            Console.WriteLine($"Лікар {i + 1}: {costs[i].Length} прийоми, сума={totals[i]} грн, середня={avg:F2} грн");
        }
        
        Console.WriteLine($"Найбільший дохід: Лікар {bestIdx + 1} ({totals[bestIdx]} грн)");    
    }
}