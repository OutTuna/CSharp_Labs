namespace ClinicApp;

public class Appointment {
    private static int _nextId = 1;
    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }
    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30) {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;

        Status = AppointmentStatus.Scheduled;
        Notes = "";
    }

    public bool Cancel(string reason = "") {
        if (Status != AppointmentStatus.Scheduled) {
            return false;
        }

        Status = AppointmentStatus.Cancelled;

        if (reason.Length > 0) {
            Notes = reason;
        }

        return true;
    }

    public bool Complete() {
        if (Status != AppointmentStatus.Scheduled) {
            return false;
        }

        Status = AppointmentStatus.Completed;
        return true;
    }

    public override string ToString() {
        string timeRange = $"{ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm}";
        string result = $"[{Id}] Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeRange} | {Status}";

        if (Notes.Length > 0) {
            result += " | " + Notes;
        }

        return result;
    }
}