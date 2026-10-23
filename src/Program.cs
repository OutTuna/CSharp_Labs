using System.Globalization;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Managers;

namespace ClinicApp;

class Program {
    static void Main() {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = Encoding.UTF8;
        Clinic clinic = new Clinic("Медична Клініка");

        SeedClinic(clinic);

        SeedMedicalRecords(clinic);

        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Пацієнти — реєстрація, пошук");
            Console.WriteLine("2. Лікарі — персонал, розклад");
            Console.WriteLine("3. Записи — створення, скасування");
            Console.WriteLine("4. Медична картка — діагнози, аналізи, рецепти");
            Console.WriteLine("5. Рахунки — оплата, борги");
            Console.WriteLine("6. Звіт клініки — статистика, навантаження");
            Console.WriteLine("7. Тест зростаючого масиву — додавання, пошук");
            Console.WriteLine("8. Розклад на дату — перегляд записів");
            Console.WriteLine("0. Вихід — завершення роботи");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1") {
                PatientsMenu(clinic);
            } else if (choice == "2") {
                DoctorsMenu(clinic);
            } else if (choice == "3") {
                AppointmentsMenu(clinic);
            } else if (choice == "4") {
                MedicalRecordsMenu(clinic);
            } else if (choice == "5") {
                BillingMenu(clinic);
            } else if (choice == "6") {
                clinic.GenerateReport();
                Pause();
            } else if (choice == "7") {
                TestGrowablePatientManager();
                Pause();
            } else if (choice == "8") {
                ShowSchedule(clinic);
                Pause();
            } else if (choice == "0") {
                break;
            } else {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    static void BillingMenu(Clinic clinic) {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("── Рахунки ───────────────────");
            Console.WriteLine("1. Борги пацієнта");
            Console.WriteLine("2. Всі неоплачені записи");
            Console.WriteLine("3. Оплатити запис");
            Console.WriteLine("4. Загальний борг клініки");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0") {
                return;
            }

            if (choice == "1") {
                clinic.Patients.DisplayAll();

                if (!ReadRecordPatient(clinic, out int patientId)) {
                    continue;
                }

                IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                clinic.Billing.DisplayUnpaid(unpaid);
                Console.WriteLine($"Борг пацієнта: {clinic.Billing.GetPatientDebt(patientId):F2}");
            } else if (choice == "2") {
                IPayable[] unpaid = clinic.Billing.GetAllUnpaid();
                clinic.Billing.DisplayUnpaid(unpaid);
            } else if (choice == "3") {
                if (!ReadRecordInt("ID запису: ", out int appointmentId)) {
                    continue;
                }

                if (clinic.Billing.PayAppointment(appointmentId)) {
                    Console.WriteLine("Запис оплачено.");
                } else {
                    Console.WriteLine("Помилка: запис не знайдено, вже оплачено або скасовано.");
                }
            } else if (choice == "4") {
                Console.WriteLine($"Загальний борг клініки: {clinic.Billing.GetTotalDebt():F2}");
            } else {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    static void SeedMedicalRecords(Clinic clinic)
    {
        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 2, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт"));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-7), "Гемоглобін", 145, "г/л", "120–160", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-7), "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Ібупрофен", "200 мг", 10));
        clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-40), "G44.2", "Головний біль напруження"));
        clinic.MedicalRecords.Add(new LabResult(3, 3, DateTime.Today, "Глюкоза", 4.8, "ммоль/л", "3.3–5.5", true));
    }

    static void MedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("── Медична картка ────────────");
            Console.WriteLine("1. Картка пацієнта");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");
            string choice = Console.ReadLine()!;
            if (choice == "0")
                return;

            if (choice == "1" || choice == "2")
            {
                clinic.Patients.DisplayAll();
                if (!ReadRecordPatient(clinic, out int patientId))
                    continue;
                if (choice == "1")
                    clinic.MedicalRecords.DisplayPatientSummary(patientId);
                else
                {
                    MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(patientId);
                    clinic.MedicalRecords.DisplayList(records);
                }
            }
            else if (choice == "3" || choice == "4" || choice == "5")
                AddMedicalRecord(clinic, choice);
            else if (choice == "6")
            {
                clinic.Doctors.DisplayAll();
                if (!ReadRecordDoctor(clinic, out int doctorId))
                    continue;
                clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(doctorId));
            }
            else
                Console.WriteLine("Невідомий пункт меню.");
        }
    }

    static bool ReadRecordInt(string prompt, out int value)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out value))
            return true;
        Console.WriteLine("Некоректне ціле число.");
        return false;
    }

    static bool ReadRecordFlag(string prompt, out bool value)
    {
        value = false;
        if (!ReadRecordInt(prompt, out int number))
            return false;
        if (number != 0 && number != 1)
        {
            Console.WriteLine("Введіть 1 або 0.");
            return false;
        }
        value = number == 1;
        return true;
    }

    static bool ReadRecordPatient(Clinic clinic, out int patientId)
    {
        if (!ReadRecordInt("ID пацієнта: ", out patientId))
            return false;
        if (clinic.Patients.FindById(patientId) != null)
            return true;
        Console.WriteLine("Пацієнта не знайдено.");
        return false;
    }

    static bool ReadRecordDoctor(Clinic clinic, out int doctorId)
    {
        if (!ReadRecordInt("ID лікаря: ", out doctorId))
            return false;
        if (clinic.Doctors.FindById(doctorId) != null)
            return true;
        Console.WriteLine("Лікаря не знайдено.");
        return false;
    }

    static string ReadRecordText(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? "").Trim();
    }

    static void AddMedicalRecord(Clinic clinic, string choice)
    {
        clinic.Patients.DisplayAll();
        clinic.Doctors.DisplayAll();
        if (!ReadRecordPatient(clinic, out int patientId))
            return;
        if (!ReadRecordDoctor(clinic, out int doctorId))
            return;

        try
        {
            if (choice == "3")
            {
                string code = ReadRecordText("Код діагнозу (напр. J06.9): ");
                string description = ReadRecordText("Опис: ");
                if (!ReadRecordFlag("Хронічне? (1=так, 0=ні): ", out bool chronic))
                    return;
                clinic.MedicalRecords.Add(new Diagnosis(patientId, doctorId, DateTime.Today, code, description, chronic));
            }
            else if (choice == "4")
            {
                string name = ReadRecordText("Назва аналізу: ");
                string input = ReadRecordText("Значення (число з крапкою): ");
                if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Некоректне число. Використовуйте крапку для дробової частини.");
                    return;
                }
                string unit = ReadRecordText("Одиниці виміру: ");
                string range = ReadRecordText("Норма (напр. 4.0–9.0): ");
                if (!ReadRecordFlag("В нормі? (1=так, 0=ні): ", out bool normal))
                    return;
                clinic.MedicalRecords.Add(new LabResult(patientId, doctorId, DateTime.Today, name, value, unit, range, normal));
            }
            else
            {
                string medication = ReadRecordText("Препарат: ");
                string dosage = ReadRecordText("Дозування (10 мг): ");
                if (!ReadRecordInt("Кількість днів: ", out int days))
                    return;
                string instructions = ReadRecordText("Інструкція (Enter — пропустити): ");
                clinic.MedicalRecords.Add(new Prescription(patientId, doctorId, DateTime.Today, medication, dosage, days, instructions));
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine("Помилка: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Помилка: " + e.Message);
        }
    }
    static void SeedClinic(Clinic clinic) {
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));

        Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        d1.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
        d2.Schedule = new WorkSchedule(9, 18);
        clinic.Doctors.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");
        clinic.Doctors.Add(d3);

        Doctor d4 = new Doctor("Марія", "Іваненко", Speciality.Cardiology, "LIC-004", "0445678901");
        d4.Schedule = new WorkSchedule(12, 20);
        clinic.Doctors.Add(d4);

        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule copy = morning;
        Console.WriteLine();
        Console.WriteLine($"[Експеримент] morning = {morning}");
        Console.WriteLine($"[Експеримент] copy    = {copy}");
        Console.WriteLine("[Експеримент] WorkSchedule — value type: copy і morning незалежні.");
        Console.WriteLine("[Експеримент] Start/End — get-only, тому copy.Start = 5 не скомпілюється.");
        Console.WriteLine();

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
        try {
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
        catch (ArgumentOutOfRangeException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
        catch (ArgumentException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
    }

    static void AddDoctor(Clinic clinic) {
        try {
            string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
            string lastName = ReadNonEmpty("Прізвище: ", "Лікар");
            Speciality speciality = ReadSpeciality();
            string licenseNumber = ReadNonEmpty("Номер ліцензії: ", "LIC-000");
            string phone = ReadNonEmpty("Телефон: ", "0000000000");

            Doctor doctor = new Doctor(firstName, lastName, speciality, licenseNumber, phone);

            Console.WriteLine("Графік роботи (залиште порожнім для 8–17):");

            string startText = ReadNonEmpty("Початок роботи (година, напр. 8): ", "");
            string endText = ReadNonEmpty("Кінець роботи (година, напр. 17): ", "");

            int startHour = 8;
            int endHour = 17;

            if (startText.Length > 0) {
                int parsedStart;

                if (int.TryParse(startText, out parsedStart)) {
                    startHour = parsedStart;
                } else {
                    Console.WriteLine("Некоректна година. Використано 8.");
                }
            }

            if (endText.Length > 0) {
                int parsedEnd;

                if (int.TryParse(endText, out parsedEnd)) {
                    endHour = parsedEnd;
                } else {
                    Console.WriteLine("Некоректна година. Використано 17.");
                }
            }

            doctor.Schedule = new WorkSchedule(startHour, endHour);

            clinic.Doctors.Add(doctor);
        }
        catch (ArgumentOutOfRangeException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
        catch (ArgumentException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
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
        try {
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

                if (int.TryParse(durationText, out parsedDuration)) {
                    durationMinutes = parsedDuration;
                } else {
                    Console.WriteLine("Некоректна тривалість. Використано 30.");
                }
            }

            clinic.Appointments.Book(patientId, doctorId, scheduledAt, durationMinutes);
        }
        catch (ArgumentOutOfRangeException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
        catch (ArgumentException e) {
            Console.WriteLine("Помилка: " + e.Message);
        }
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
