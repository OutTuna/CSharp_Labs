namespace ClinicApp.Models;

public class UrgentAppointment : Appointment {
    public string UrgencyNote { get; }

    public UrgentAppointment(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
        : base(patientId, doctorId, scheduledAt, durationMinutes) {
        UrgencyNote = urgencyNote;
    }

    public override decimal GetCost() {
        return base.GetCost() * 1.5m;
    }

    public sealed override string GetDescription() {
        if (string.IsNullOrWhiteSpace(UrgencyNote)) {
            return "Терміновий";
        }

        return "Терміновий (" + UrgencyNote + ")";
    }

    public new int GetPriority() {
        return 1;
    }
}
