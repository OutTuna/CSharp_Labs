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
        Patient? patient = _patients.FindById(patientId);

        if (patient == null) {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(doctorId);

        if (doctor == null) {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count == MaxAppointments) {
            Console.WriteLine("Ліміт записів вичерпано.");
            return false;
        }

        Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);

        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");

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
        string result = $"[{appointment.Id}] {patientName} → {doctorName} | {timeRange} | {appointment.Status}";

        if (appointment.Notes.Length > 0) {
            result += " | " + appointment.Notes;
        }

        Console.WriteLine(result);
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