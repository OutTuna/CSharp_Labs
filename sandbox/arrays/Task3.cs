using System;

public class Task3
{
    public static void Run()
    {
        string[] days = { "Понеділок", "Вівторок", "Середа", "Четвер", "Пʼятниця", "Субота", "Неділя" };

        int[] patients = new int[7];
        for (int i = 0; i < 7; i++)
        {
            patients[i] = int.Parse(Console.ReadLine()!);
        }

        int total = 0;
        int maxIdx = 0;
        int minIdx = 0;

        for (int i = 0; i < 7; i++)
        {
            total += patients[i];
            if (patients[i] > patients[maxIdx]) maxIdx = i;
            if (patients[i] <  patients[minIdx]) minIdx = i;
        }
        
        for (int i = 0; i < 7; i++)
        { 
            Console.WriteLine($"{days[i],-12}: {patients[i]} пацієнтів"); 
        }
        
        Console.WriteLine($"{"Разом:",-14}{total}");
        Console.WriteLine($"{"Найбільше:",-14}{days[maxIdx]} ({patients[maxIdx]})");
        Console.WriteLine($"{"Найменше:",-14}{days[minIdx]} ({patients[minIdx]})");

    }
}