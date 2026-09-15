using System;
public static class Task7
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);

        string[] names = new string[n];
        double[] bmis = new double[n];

        for (int i = 0; i < n; i++)
        {
            names[i] = Console.ReadLine()!;
            bmis[i] = double.Parse(Console.ReadLine()!);
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (bmis[j] < bmis[j + 1])
                {
                    (bmis[j], bmis[j + 1]) = (bmis[j + 1], bmis[j]);
                    (names[j], names[j + 1]) = (names[j + 1], names[j]);
                }
            }
        }

        Console.WriteLine("=== Рейтинг ІМТ ===");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"#{i + 1} {names[i]}: {bmis[i]:F2}");
        }
    }
}