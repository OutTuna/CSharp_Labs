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
    }
}