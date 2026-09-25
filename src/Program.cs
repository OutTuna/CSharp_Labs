namespace ClinicApp;

class Program
{
    static void Main()
    {
        PatientManager patients = new PatientManager();

        SeedPatients(patients);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1")
            {
                PatientsMenu(patients);
            }
            else if (choice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    static void SeedPatients(PatientManager patients)
    {
        patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 10), "A+", "0501234567"));
        patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 22), "B-", "0672345678"));
        patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), "O+", "0933456789"));
        patients.Add(new Patient("Марія", "Ткач"));
    }

    static void PatientsMenu(PatientManager patients)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0")
            {
                return;
            }

            if (choice == "1")
            {
                patients.DisplayAll();
            }
            else if (choice == "2")
            {
                AddPatient(patients);
            }
            else if (choice == "3")
            {
                FindPatientsByName(patients);
            }
            else if (choice == "4")
            {
                RemovePatientById(patients);
            }
            else if (choice == "5")
            {
                patients.DisplayStats();
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void AddPatient(PatientManager patients)
    {
        Console.WriteLine();
        Console.WriteLine("Додавання пацієнта");

        string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
        string lastName = ReadNonEmpty("Прізвище: ", "Пацієнт");

        string dateText = ReadNonEmpty("Дата народження (дд.мм.рррр): ", "");

        DateTime dateOfBirth;

        if (!DateTime.TryParse(dateText, out dateOfBirth))
        {
            dateOfBirth = new DateTime(2000, 1, 1);
            Console.WriteLine("Дата розпізнана некоректно. Використано 01.01.2000.");
        }

        string bloodType = ReadNonEmpty("Група крові: ", "Невідомо");
        string phone = ReadNonEmpty("Телефон: ", "0000000000");

        Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);

        patients.Add(patient);
    }

    static void FindPatientsByName(PatientManager patients)
    {
        string query = ReadNonEmpty("Введіть частину імені або прізвища: ", "");

        if (query.Length == 0)
        {
            Console.WriteLine("Пошуковий запит порожній.");
            return;
        }

        Patient[] found = patients.FindByName(query);

        if (found.Length == 0)
        {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        for (int i = 0; i < found.Length; i++)
        {
            Console.WriteLine(found[i]);
        }
    }

    static void RemovePatientById(PatientManager patients)
    {
        string input = ReadNonEmpty("Введіть ID пацієнта для видалення: ", "");

        int id;

        if (!int.TryParse(input, out id))
        {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        if (patients.Remove(id))
        {
            Console.WriteLine($"Пацієнта з ID {id} видалено.");
        }
        else
        {
            Console.WriteLine($"Пацієнта з ID {id} не знайдено.");
        }
    }

    static string ReadNonEmpty(string prompt, string defaultValue)
    {
        Console.Write(prompt);

        string input = Console.ReadLine()!;

        input = input.Trim();

        if (input.Length == 0)
        {
            return defaultValue;
        }

        return input;
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.Write("Натисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }
}