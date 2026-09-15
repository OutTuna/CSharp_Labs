using System;
public static class Task5
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);

        int[,] matrix = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        int[] mainDiag = new int[n];
        int[] secDiag = new int[n];
        int mainSum = 0;
        int secSum = 0;

        for (int i = 0; i < n; i++)
        {
            mainDiag[i] = matrix[i, i];
            secDiag[i] = matrix[i, n - 1 - i];
            mainSum += mainDiag[i];
            secSum += secDiag[i];
        }

        Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiag)} (сума = {mainSum})");
        Console.WriteLine($"Побічна діагональ: {string.Join(", ", secDiag)} (сума = {secSum})");
    }
}