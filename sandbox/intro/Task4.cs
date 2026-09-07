using System;
    public static class Task4
    {
        public static void Run()
        {
            Console.WriteLine("systolic: ");
            int systolic = int.Parse(Console.ReadLine()!);
            Console.WriteLine("diastolic: ");
            int diastolic = int.Parse(Console.ReadLine()!);

            string status;

            if (systolic < 120 && diastolic < 80)
            {
                status = "Kaif";
            }

            else if (systolic < 130 && diastolic < 80)
            {
                status = "chuchut ne kaif";
            }

            else if (systolic < 140 || diastolic < 90)
            {
                status = "gipertonia 1 lvl";
            }
            else
            {
                status = "gipertonia 2 lvl";
            }

            Console.WriteLine($"Blood Pressure: {systolic}/{diastolic} - {status}");
        }
    }