namespace ClinicApp.Models;

public class UrgentAppointment : Appointment {
    private readonly decimal _multiplier;
    public string UrgencyNote { get; }

    public UrgentAppointment(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30, decimal multiplier = 1.5m)
        : base(patientId, doctorId, scheduledAt, durationMinutes) {
        UrgencyNote = urgencyNote;
        _multiplier = multiplier;
    }

    public override decimal GetCost() {
        return base.GetCost() * _multiplier;
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
