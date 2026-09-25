namespace ClinicApp;

class Program
{
    static void Main()
    {
        Patient p1 = new Patient("Іван", "Петренко", new DateTime(1985, 5, 10), "A+", "0501234567");
        Patient p2 = new Patient("Олена", "Коваль", new DateTime(1993, 8, 22), "B-", "0672345678");
        Patient p3 = new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), "O+", "0933456789");
        Patient p4 = new Patient();
        Patient p5 = new Patient("Марія", "Ткач");

        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        Console.WriteLine(p4);
        Console.WriteLine(p5);

        Console.WriteLine();
        
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

        Doctor d4 = new Doctor("Марія", "Іваненко", "Терапія", "LIC-004", "0445678901");
        d4.WorkStartHour = 12;
        d4.WorkEndHour = 20;

        Console.WriteLine(d1);
        Console.WriteLine(d2);
        Console.WriteLine(d3);
        Console.WriteLine(d4);

        Console.WriteLine();
        
        Console.WriteLine($"{d1.FullName} о 10:00 — {(d1.CanAcceptAt(10) ? "може прийняти" : "не може прийняти")}");
        Console.WriteLine($"{d2.FullName} о 10:00 — {(d2.CanAcceptAt(10) ? "може прийняти" : "не може прийняти")}");
        Console.WriteLine($"{d3.FullName} о 10:00 — {(d3.CanAcceptAt(10) ? "може прийняти" : "не може прийняти")}");
        Console.WriteLine($"{d4.FullName} о 10:00 — {(d4.CanAcceptAt(10) ? "може прийняти" : "не може прийняти")}");
    }
}