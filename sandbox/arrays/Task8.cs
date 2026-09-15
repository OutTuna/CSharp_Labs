using System;
public static class Task8
{
    public static void Run()
    {
        int d = int.Parse(Console.ReadLine()!);
        int w = int.Parse(Console.ReadLine()!);

        int[,,] data = new int[d, w, 2];

        for (int dept = 0; dept < d; dept++)
        {
            for (int week = 0; week < w; week++)
            {
                for (int shift = 0; shift < 2; shift++)
                {
                    data[dept, week, shift] = int.Parse(Console.ReadLine()!);
                }
            }
        }

        int[] totals = new int[d];
        int bestIdx = 0;

        for (int dept = 0; dept < d; dept++)
        {
            Console.WriteLine($"Відділення {dept + 1}:");
            for (int week = 0; week < w; week++)
            {
                int morning = data[dept, week, 0];
                int evening = data[dept, week, 1];
                int weekTotal = morning + evening;
                totals[dept] += weekTotal;
                Console.WriteLine($"  Тиждень {week + 1}: ранок {morning}, вечір {evening} → разом {weekTotal}");
            }
            Console.WriteLine($"  Разом: {totals[dept]} пацієнтів");

            if (totals[dept] > totals[bestIdx]) bestIdx = dept;
        }

        Console.WriteLine($"Найзавантаженіше: Відділення {bestIdx + 1} ({totals[bestIdx]} пацієнтів)");
    }
}