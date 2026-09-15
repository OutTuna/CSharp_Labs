using System;

public static class Task4
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int m = int.Parse(Console.ReadLine()!);
        
        int[,]matrix = new int[n, m];
        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < m; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }
        
        int[] rowSums = new int[n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                rowSums[i] += matrix[i, j];
            }
        }
 
        int[] colSums = new int[m];
        for (int j = 0; j < m; j++)
        {
            for (int i = 0; i < n; i++)
            {
                colSums[j] += matrix[i, j];
            }
        }

        
        int max = matrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (matrix[i, j] > max)
                {
                    max = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
        }
 
        for (int i = 0; i < n; i++)
        { 
            Console.WriteLine($"Лікар {i + 1}: {rowSums[i]} прийомів");
        }
 
        Console.WriteLine($"По днях: {string.Join(", ", colSums)}");
        Console.WriteLine($"Максимум: {max} (Лікар {maxRow + 1}, День {maxCol + 1})");

        
    }
}