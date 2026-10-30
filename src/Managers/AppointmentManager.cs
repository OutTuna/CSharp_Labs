using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentManager {
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;
    private PatientManager _patients;
    private DoctorManager _doctors;
    public int Count => _count;

    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) {
                return null;
            }

            return _appointments[index];
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors) {
        _patients = patients;
        _doctors = doctors;
    }

    private Appointment? FindById(int id) {
        for (int i = 0; i < _count; i++) {
            if (_appointments[i].Id == id) {
                return _appointments[i];
            }
        }

        return null;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30) {
        return AddAppointment(new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes));
    }

    public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30) {
        return AddAppointment(new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes));
    }

    public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45) {
        return AddAppointment(new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes));
    }

    private bool AddAppointment(Appointment appointment) {
        Patient? patient = _patients.FindById(appointment.PatientId);

        if (patient == null) {
            Console.WriteLine($"Помилка: пацієнта з ID {appointment.PatientId} не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        if (doctor == null) {
            Console.WriteLine($"Помилка: лікаря з ID {appointment.DoctorId} не знайдено.");
            return false;
        }

        if (_count == MaxAppointments) {
            Console.WriteLine("Ліміт записів вичерпано.");
            return false;
        }

        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");

        return true;
    }

    public bool Cancel(int id, string reason) {
        Appointment? appointment = FindById(id);

        if (appointment == null) {
            Console.WriteLine($"Запис з ID {id} не знайдено.");
            return false;
        }

        bool success = appointment.Cancel(reason);

        if (success) {
            Console.WriteLine($"Запис [{id}] скасовано.");
        } else {
            Console.WriteLine($"Запис [{id}] вже скасовано або завершено.");
        }

        return success;
    }

    public bool Complete(int id) {
        Appointment? appointment = FindById(id);

        if (appointment == null) {
            Console.WriteLine($"Запис з ID {id} не знайдено.");
            return false;
        }

        bool success = appointment.Complete();

        if (success) {
            Console.WriteLine($"Запис [{id}] завершено.");
        } else {
            Console.WriteLine($"Запис [{id}] вже скасовано або завершено.");
        }

        return success;
    }

    public Appointment[] GetAll() {
        Appointment[] result = new Appointment[_count];

        for (int i = 0; i < _count; i++) {
            result[i] = _appointments[i];
        }

        return result;
    }

    public Appointment[] GetByPatient(int patientId) {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].PatientId == patientId) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].PatientId == patientId) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDoctor(int doctorId) {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].DoctorId == doctorId) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].DoctorId == doctorId) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDate(DateTime date) {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].ScheduledAt.Date == date.Date) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].ScheduledAt.Date == date.Date) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    // Overload: три числа замість DateTime
    public Appointment[] GetByDate(int year, int month, int day) {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming() {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].IsUpcoming) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i].IsUpcoming) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetUrgent() {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is UrgentAppointment) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is UrgentAppointment) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetSpecialist() {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is SpecialistAppointment) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is SpecialistAppointment) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetRegular() {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is RegularAppointment) {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_appointments[i] is RegularAppointment) {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public void DisplayAppointment(Appointment appointment) {
        Patient? patient = _patients.FindById(appointment.PatientId);

        string patientName;

        if (patient != null) {
            patientName = patient.FullName;
        } else {
            patientName = "Пацієнт #" + appointment.PatientId;
        }

        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        string doctorName;

        if (doctor != null) {
            doctorName = doctor.FullName;
        } else {
            doctorName = "Лікар #" + appointment.DoctorId;
        }

        string timeRange = $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm}";
        string result = $"[{appointment.Id}] {appointment.GetDescription()} | {patientName} → {doctorName} | {timeRange} | {appointment.Status}";

        if (appointment.Notes.Length > 0) {
            result += " | " + appointment.Notes;
        }

        Console.WriteLine(result + $" | {appointment.GetCost():F2} грн");
    }

    public void DisplayList(Appointment[] appointments) {
        if (appointments.Length == 0) {
            Console.WriteLine("Записів не знайдено.");
            return;
        }
        for (int i = 0; i < appointments.Length; i++) {
            DisplayAppointment(appointments[i]);
        }
    }
}