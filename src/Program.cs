namespace ClinicApp;

class Program {
    static void Main() {
        Clinic clinic = new Clinic("Медична Клініка");

        SeedClinic(clinic);

        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("4. Розклад на дату");
            Console.WriteLine("5. Звіт клініки");
            Console.WriteLine("6. Тест зростаючого масиву");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1") {
                PatientsMenu(clinic);
            } else if (choice == "2") {
                DoctorsMenu(clinic);
            } else if (choice == "3") {
                AppointmentsMenu(clinic);
            } else if (choice == "4") {
                ShowSchedule(clinic);
                Pause();
            } else if (choice == "5") {
                clinic.GenerateReport();
                Pause();
            } else if (choice == "6") {
                TestGrowablePatientManager();
                Pause();
            } else if (choice == "0") {
                break;
            } else {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    static void SeedClinic(Clinic clinic) {
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        clinic.Doctors.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");
        clinic.Doctors.Add(d3);

        Doctor d4 = new Doctor("Марія", "Іваненко", Speciality.Cardiology, "LIC-004", "0445678901");
        d4.WorkStartHour = 12;
        d4.WorkEndHour = 20;
        clinic.Doctors.Add(d4);

        DateTime tomorrow = DateTime.Today.AddDays(1);

        clinic.Appointments.Book(1, 1, tomorrow.AddHours(10), 30);
        clinic.Appointments.Book(2, 2, tomorrow.AddHours(11), 45);
        clinic.Appointments.Book(3, 3, tomorrow.AddDays(1).AddHours(9), 20);
    }

    static void PatientsMenu(Clinic clinic) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0") {
                return;
            }

            if (choice == "1") {
                clinic.Patients.DisplayAll();
            } else if (choice == "2") {
                AddPatient(clinic);
            } else if (choice == "3") {
                FindPatientsByName(clinic);
            } else if (choice == "4") {
                RemovePatientById(clinic);
            } else if (choice == "5") {
                clinic.Patients.DisplayStats();
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void DoctorsMenu(Clinic clinic) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine("3. Знайти за спеціальністю");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0") {
                return;
            }

            if (choice == "1") {
                clinic.Doctors.DisplayAll();
            } else if (choice == "2") {
                AddDoctor(clinic);
            } else if (choice == "3") {
                FindDoctorsBySpeciality(clinic);
            } else if (choice == "4") {
                RemoveDoctorById(clinic);
            } else if (choice == "5") {
                clinic.Doctors.DisplayStats();
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void AppointmentsMenu(Clinic clinic) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Показати майбутні записи");
            Console.WriteLine("2. Показати записи пацієнта");
            Console.WriteLine("3. Показати записи лікаря");
            Console.WriteLine("4. Показати записи на дату");
            Console.WriteLine("5. Створити запис");
            Console.WriteLine("6. Скасувати запис");
            Console.WriteLine("7. Завершити запис");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0") {
                return;
            }

            if (choice == "1") {
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
            } else if (choice == "2") {
                ShowAppointmentsByPatient(clinic);
            } else if (choice == "3") {
                ShowAppointmentsByDoctor(clinic);
            } else if (choice == "4") {
                ShowAppointmentsByDate(clinic);
            } else if (choice == "5") {
                BookAppointment(clinic);
            } else if (choice == "6") {
                CancelAppointment(clinic);
            } else if (choice == "7") {
                CompleteAppointment(clinic);
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void ShowSchedule(Clinic clinic) {
        string dateText = ReadNonEmpty("Введіть дату (дд.мм.рррр): ", "");

        DateTime date;

        if (!DateTime.TryParse(dateText, out date)) {
            Console.WriteLine("Некоректна дата.");
            return;
        }

        clinic.DisplaySchedule(date);
    }

    static void TestGrowablePatientManager() {
        GrowablePatientManager manager = new GrowablePatientManager();

        Console.WriteLine();
        Console.WriteLine("Тест GrowablePatientManager");
        Console.WriteLine("Додаємо пацієнтів одного за одним...");

        bool tenthSaved = false;
        int tenthId = 0;

        for (int i = 1; i <= 20; i++) {
            Patient patient = new Patient("Тест", "Пацієнт" + i, new DateTime(1990, 1, 1), BloodType.APositive, "0000000000");

            if (i == 10) {
                tenthSaved = true;
                tenthId = patient.Id;
            }

            manager.Add(patient);

            Console.WriteLine($"Додано [{patient.Id}]. Розмір: {manager.Count} / {manager.Capacity}");
        }

        Console.WriteLine();
        Console.WriteLine("Тест пошуку:");

        if (tenthSaved) {
            Patient found = manager.FindById(tenthId);

            if (found == null) {
                Console.WriteLine($"FindById({tenthId}) → не знайдено");
            } else {
                Console.WriteLine($"FindById({tenthId}) → {found.FullName}");
            }
        }

        Patient notFound = manager.FindById(999999);

        if (notFound == null) {
            Console.WriteLine("FindById(999999) → не знайдено");
        } else {
            Console.WriteLine($"FindById(999999) → {notFound.FullName}");
        }

        Console.WriteLine();
        Console.WriteLine("Порівняння:");
        Console.WriteLine("PatientManager:         100 місць (фіксовано)");
        Console.WriteLine($"GrowablePatientManager: {manager.Capacity} місця (зросте при потребі)");

        Console.WriteLine();
        manager.DisplayAll();
    }

    static void AddPatient(Clinic clinic) {
        string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
        string lastName = ReadNonEmpty("Прізвище: ", "Пацієнт");

        string dateText = ReadNonEmpty("Дата народження (дд.мм.рррр): ", "");

        DateTime dateOfBirth;

        if (!DateTime.TryParse(dateText, out dateOfBirth)) {
            dateOfBirth = new DateTime(2000, 1, 1);
            Console.WriteLine("Дата розпізнана некоректно. Використано 01.01.2000.");
        }

        BloodType bloodType = ReadBloodType();
        string phone = ReadNonEmpty("Телефон: ", "0000000000");

        Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);

        clinic.Patients.Add(patient);
    }

    static void AddDoctor(Clinic clinic) {
        string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
        string lastName = ReadNonEmpty("Прізвище: ", "Лікар");
        Speciality speciality = ReadSpeciality();
        string licenseNumber = ReadNonEmpty("Номер ліцензії: ", "LIC-000");
        string phone = ReadNonEmpty("Телефон: ", "0000000000");

        Doctor doctor = new Doctor(firstName, lastName, speciality, licenseNumber, phone);

        Console.WriteLine("Графік роботи (залиште порожнім для 8–17):");

        string startText = ReadNonEmpty("Початок роботи (година, напр. 8): ", "");

        if (startText.Length > 0) {
            int workStart;

            if (int.TryParse(startText, out workStart) && workStart >= 0 && workStart <= 23) {
                doctor.WorkStartHour = workStart;
            } else {
                Console.WriteLine("Некоректна година. Використано 8.");
            }
        }

        string endText = ReadNonEmpty("Кінець роботи (година, напр. 17): ", "");

        if (endText.Length > 0) {
            int workEnd;

            if (int.TryParse(endText, out workEnd) && workEnd >= 0 && workEnd <= 23) {
                doctor.WorkEndHour = workEnd;
            } else {
                Console.WriteLine("Некоректна година. Використано 17.");
            }
        }

        clinic.Doctors.Add(doctor);
    }

    static BloodType ReadBloodType() {
        BloodType[] values = Enum.GetValues<BloodType>();

        Console.WriteLine("Група крові:");
        for (int i = 0; i < values.Length; i++) {
            Console.WriteLine($"  {i}. {values[i]}");
        }

        string input = ReadNonEmpty("Виберіть номер: ", "0");
        int index;

        if (!int.TryParse(input, out index) || index < 0 || index >= values.Length) {
            Console.WriteLine("Некоректний вибір. Використано Unknown.");
            return BloodType.Unknown;
        }

        return (BloodType)index;
    }

    static Speciality ReadSpeciality() {
        Speciality[] values = Enum.GetValues<Speciality>();

        Console.WriteLine("Спеціальність:");
        for (int i = 0; i < values.Length; i++) {
            Console.WriteLine($"  {i}. {values[i]}");
        }

        string input = ReadNonEmpty("Виберіть номер: ", "0");
        int index;

        if (!int.TryParse(input, out index) || index < 0 || index >= values.Length) {
            Console.WriteLine("Некоректний вибір. Використано General.");
            return Speciality.General;
        }

        return (Speciality)index;
    }

    static void FindPatientsByName(Clinic clinic) {
        string query = ReadNonEmpty("Введіть частину імені або прізвища: ", "");

        if (query.Length == 0) {
            Console.WriteLine("Пошуковий запит порожній.");
            return;
        }

        Patient[] found = clinic.Patients.FindByName(query);

        if (found.Length == 0) {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        for (int i = 0; i < found.Length; i++) {
            Console.WriteLine(found[i]);
        }
    }

    static void FindDoctorsBySpeciality(Clinic clinic) {
        string query = ReadNonEmpty("Введіть частину спеціальності: ", "");

        if (query.Length == 0) {
            Console.WriteLine("Пошуковий запит порожній.");
            return;
        }

        Doctor[] found = clinic.Doctors.FindBySpeciality(query);

        if (found.Length == 0) {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        for (int i = 0; i < found.Length; i++) {
            Console.WriteLine(found[i]);
        }
    }

    static void RemovePatientById(Clinic clinic) {
        string input = ReadNonEmpty("Введіть ID пацієнта для видалення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        if (clinic.Patients.Remove(id)) {
            Console.WriteLine($"Пацієнта з ID {id} видалено.");
        } else {
            Console.WriteLine($"Пацієнта з ID {id} не знайдено.");
        }
    }

    static void RemoveDoctorById(Clinic clinic) {
        string input = ReadNonEmpty("Введіть ID лікаря для видалення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        if (clinic.Doctors.Remove(id)) {
            Console.WriteLine($"Лікаря з ID {id} видалено.");
        } else {
            Console.WriteLine($"Лікаря з ID {id} не знайдено.");
        }
    }

    static void ShowAppointmentsByPatient(Clinic clinic) {
        clinic.Patients.DisplayAll();

        string input = ReadNonEmpty("Введіть ID пацієнта: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(id));
    }

    static void ShowAppointmentsByDoctor(Clinic clinic) {
        clinic.Doctors.DisplayAll();

        string input = ReadNonEmpty("Введіть ID лікаря: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(id));
    }

    static void ShowAppointmentsByDate(Clinic clinic) {
        string dateText = ReadNonEmpty("Введіть дату (дд.мм.рррр): ", "");

        DateTime date;

        if (!DateTime.TryParse(dateText, out date)) {
            Console.WriteLine("Некоректна дата.");
            return;
        }

        clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(date));
    }

    static void BookAppointment(Clinic clinic) {
        clinic.Patients.DisplayAll();

        string patientInput = ReadNonEmpty("Введіть ID пацієнта: ", "");

        int patientId;

        if (!int.TryParse(patientInput, out patientId)) {
            Console.WriteLine("Некоректний ID пацієнта.");
            return;
        }

        clinic.Doctors.DisplayAll();

        string doctorInput = ReadNonEmpty("Введіть ID лікаря: ", "");

        int doctorId;

        if (!int.TryParse(doctorInput, out doctorId)) {
            Console.WriteLine("Некоректний ID лікаря.");
            return;
        }

        string dateText = ReadNonEmpty("Введіть дату (дд.мм.рррр): ", "");

        DateTime date;

        if (!DateTime.TryParse(dateText, out date)) {
            Console.WriteLine("Некоректна дата.");
            return;
        }

        string timeText = ReadNonEmpty("Введіть час (гг:хх): ", "");

        TimeSpan time;

        if (!TimeSpan.TryParse(timeText, out time)) {
            Console.WriteLine("Некоректний час.");
            return;
        }

        DateTime scheduledAt = date.Date + time;

        string durationText = ReadNonEmpty("Введіть тривалість у хвилинах (30 за замовчуванням): ", "");

        int durationMinutes = 30;

        if (durationText.Length > 0) {
            int parsedDuration;

            if (int.TryParse(durationText, out parsedDuration) && parsedDuration > 0) {
                durationMinutes = parsedDuration;
            } else {
                Console.WriteLine("Некоректна тривалість. Використано 30.");
            }
        }

        clinic.Appointments.Book(patientId, doctorId, scheduledAt, durationMinutes);
    }

    static void CancelAppointment(Clinic clinic) {
        string input = ReadNonEmpty("Введіть ID запису для скасування: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        string reason = ReadNonEmpty("Введіть причину (або залиште порожнім): ", "");

        clinic.Appointments.Cancel(id, reason);
    }

    static void CompleteAppointment(Clinic clinic) {
        string input = ReadNonEmpty("Введіть ID запису для завершення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        clinic.Appointments.Complete(id);
    }

    static string ReadNonEmpty(string prompt, string defaultValue) {
        Console.Write(prompt);

        string input = Console.ReadLine()!;

        input = input.Trim();

        if (input.Length == 0) {
            return defaultValue;
        }

        return input;
    }

    static void Pause() {
        Console.WriteLine();
        Console.Write("Натисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }
}