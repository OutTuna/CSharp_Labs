namespace ClinicApp.Models;

public sealed class SpecialistAppointment : Appointment {
    private readonly decimal _multiplier;
    public string ConsultationTopic { get; }

    public SpecialistAppointment(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45, decimal multiplier = 1.3m)
        : base(patientId, doctorId, scheduledAt, durationMinutes) {
        ConsultationTopic = topic;
        _multiplier = multiplier;
    }

    public override decimal GetCost() {
        return base.GetCost() * _multiplier;
    }

    public override string GetDescription() {
        if (string.IsNullOrWhiteSpace(ConsultationTopic)) {
            return "Консультація спеціаліста";
        }

        return "Консультація спеціаліста: " + ConsultationTopic;
    }
}
