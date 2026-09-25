namespace ClinicApp;

class Program {
    static void Main() {
        PatientManager patients = new PatientManager();
        DoctorManager doctors = new DoctorManager();
        AppointmentManager appointments = new AppointmentManager(patients, doctors);

        SeedPatients(patients);
        SeedDoctors(doctors);
        SeedAppointments(appointments);

        while (true) {
            Console.WriteLine();
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "1") {
                PatientsMenu(patients);
            } else if (choice == "2") {
                DoctorsMenu(doctors);
            } else if (choice == "3") {
                AppointmentsMenu(appointments, patients, doctors);
            } else if (choice == "0") {
                break;
            } else {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    static void SeedPatients(PatientManager patients) {
        patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 10), "A+", "0501234567"));
        patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 22), "B-", "0672345678"));
        patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), "O+", "0933456789"));
        patients.Add(new Patient("Марія", "Ткач"));
    }

    static void SeedDoctors(DoctorManager doctors) {
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        doctors.Add(d1);

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        doctors.Add(d2);

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
        doctors.Add(d3);

        Doctor d4 = new Doctor("Марія", "Іваненко", "Кардіологія", "LIC-004", "0445678901");
        d4.WorkStartHour = 12;
        d4.WorkEndHour = 20;
        doctors.Add(d4);
    }

    static void SeedAppointments(AppointmentManager appointments) {
        DateTime tomorrow = DateTime.Today.AddDays(1);

        appointments.Book(1, 1, tomorrow.AddHours(10), 30);
        appointments.Book(2, 2, tomorrow.AddHours(11), 45);
        appointments.Book(3, 3, tomorrow.AddDays(1).AddHours(9), 20);
    }

    static void PatientsMenu(PatientManager patients) {
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
                patients.DisplayAll();
            } else if (choice == "2") {
                AddPatient(patients);
            } else if (choice == "3") {
                FindPatientsByName(patients);
            } else if (choice == "4") {
                RemovePatientById(patients);
            } else if (choice == "5") {
                patients.DisplayStats();
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void DoctorsMenu(DoctorManager doctors) {
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
                doctors.DisplayAll();
            } else if (choice == "2") {
                AddDoctor(doctors);
            } else if (choice == "3") {
                FindDoctorsBySpeciality(doctors);
            } else if (choice == "4") {
                RemoveDoctorById(doctors);
            } else if (choice == "5") {
                doctors.DisplayStats();
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void AppointmentsMenu(AppointmentManager appointments, PatientManager patients, DoctorManager doctors) {
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
                appointments.DisplayList(appointments.GetUpcoming());
            } else if (choice == "2") {
                ShowAppointmentsByPatient(appointments, patients);
            } else if (choice == "3") {
                ShowAppointmentsByDoctor(appointments, doctors);
            } else if (choice == "4") {
                ShowAppointmentsByDate(appointments);
            } else if (choice == "5") {
                BookAppointment(appointments, patients, doctors);
            } else if (choice == "6") {
                CancelAppointment(appointments);
            } else if (choice == "7") {
                CompleteAppointment(appointments);
            } else {
                Console.WriteLine("Невідомий пункт меню.");
                continue;
            }

            Pause();
        }
    }

    static void AddPatient(PatientManager patients) {
        string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
        string lastName = ReadNonEmpty("Прізвище: ", "Пацієнт");

        string dateText = ReadNonEmpty("Дата народження (дд.мм.рррр): ", "");

        DateTime dateOfBirth;

        if (!DateTime.TryParse(dateText, out dateOfBirth)) {
            dateOfBirth = new DateTime(2000, 1, 1);
            Console.WriteLine("Дата розпізнана некоректно. Використано 01.01.2000.");
        }

        string bloodType = ReadNonEmpty("Група крові: ", "Невідомо");
        string phone = ReadNonEmpty("Телефон: ", "0000000000");

        Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);

        patients.Add(patient);
    }

    static void AddDoctor(DoctorManager doctors) {
        string firstName = ReadNonEmpty("Ім'я: ", "Невідомий");
        string lastName = ReadNonEmpty("Прізвище: ", "Лікар");
        string speciality = ReadNonEmpty("Спеціальність: ", "Невідомо");
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

        doctors.Add(doctor);
    }

    static void FindPatientsByName(PatientManager patients) {
        string query = ReadNonEmpty("Введіть частину імені або прізвища: ", "");

        if (query.Length == 0) {
            Console.WriteLine("Пошуковий запит порожній.");
            return;
        }

        Patient[] found = patients.FindByName(query);

        if (found.Length == 0) {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        for (int i = 0; i < found.Length; i++) {
            Console.WriteLine(found[i]);
        }
    }

    static void FindDoctorsBySpeciality(DoctorManager doctors) {
        string query = ReadNonEmpty("Введіть частину спеціальності: ", "");

        if (query.Length == 0) {
            Console.WriteLine("Пошуковий запит порожній.");
            return;
        }

        Doctor[] found = doctors.FindBySpeciality(query);

        if (found.Length == 0) {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        for (int i = 0; i < found.Length; i++) {
            Console.WriteLine(found[i]);
        }
    }

    static void RemovePatientById(PatientManager patients) {
        string input = ReadNonEmpty("Введіть ID пацієнта для видалення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        if (patients.Remove(id)) {
            Console.WriteLine($"Пацієнта з ID {id} видалено.");
        } else {
            Console.WriteLine($"Пацієнта з ID {id} не знайдено.");
        }
    }

    static void RemoveDoctorById(DoctorManager doctors) {
        string input = ReadNonEmpty("Введіть ID лікаря для видалення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        if (doctors.Remove(id)) {
            Console.WriteLine($"Лікаря з ID {id} видалено.");
        } else {
            Console.WriteLine($"Лікаря з ID {id} не знайдено.");
        }
    }

    static void ShowAppointmentsByPatient(AppointmentManager appointments, PatientManager patients) {
        patients.DisplayAll();

        string input = ReadNonEmpty("Введіть ID пацієнта: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        appointments.DisplayList(appointments.GetByPatient(id));
    }

    static void ShowAppointmentsByDoctor(AppointmentManager appointments, DoctorManager doctors) {
        doctors.DisplayAll();

        string input = ReadNonEmpty("Введіть ID лікаря: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        appointments.DisplayList(appointments.GetByDoctor(id));
    }

    static void ShowAppointmentsByDate(AppointmentManager appointments) {
        string dateText = ReadNonEmpty("Введіть дату (дд.мм.рррр): ", "");

        DateTime date;

        if (!DateTime.TryParse(dateText, out date)) {
            Console.WriteLine("Некоректна дата.");
            return;
        }

        appointments.DisplayList(appointments.GetByDate(date));
    }

    static void BookAppointment(AppointmentManager appointments, PatientManager patients, DoctorManager doctors) {
        patients.DisplayAll();

        string patientInput = ReadNonEmpty("Введіть ID пацієнта: ", "");

        int patientId;

        if (!int.TryParse(patientInput, out patientId)) {
            Console.WriteLine("Некоректний ID пацієнта.");
            return;
        }

        doctors.DisplayAll();

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

        appointments.Book(patientId, doctorId, scheduledAt, durationMinutes);
    }

    static void CancelAppointment(AppointmentManager appointments) {
        string input = ReadNonEmpty("Введіть ID запису для скасування: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        string reason = ReadNonEmpty("Введіть причину (або залиште порожнім): ", "");

        appointments.Cancel(id, reason);
    }

    static void CompleteAppointment(AppointmentManager appointments) {
        string input = ReadNonEmpty("Введіть ID запису для завершення: ", "");

        int id;

        if (!int.TryParse(input, out id)) {
            Console.WriteLine("Некоректний ID.");
            return;
        }

        appointments.Complete(id);
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