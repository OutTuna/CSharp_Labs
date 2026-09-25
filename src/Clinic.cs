namespace ClinicApp;

public class Clinic {
    public string Name { get; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name) {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date) {
        Console.WriteLine($"Розклад на {date:dd.MM.yyyy}");

        Appointment[] appointments = Appointments.GetByDate(date);

        Appointments.DisplayList(appointments);
    }

    public void GenerateReport() {
        Appointment[] upcoming = Appointments.GetUpcoming();
        Doctor[] allDoctors = Doctors.GetAll();
        Console.WriteLine($"Звіт — {Name}");
        Console.WriteLine($"Пацієнтів: {Patients.Count}");
        Console.WriteLine($"Лікарів: {Doctors.Count}");
        Console.WriteLine($"Майбутніх записів: {upcoming.Length}");
        Console.WriteLine("Навантаження лікарів (майбутні записи):");

        for (int i = 0; i < allDoctors.Length; i++) {
            int doctorCount = 0;

            for (int j = 0; j < upcoming.Length; j++) {
                if (upcoming[j].DoctorId == allDoctors[i].Id) {
                    doctorCount++;
                }
            }
            Console.WriteLine($"  {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {doctorCount} записів");
        }
    }
}