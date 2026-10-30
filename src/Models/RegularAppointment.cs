namespace ClinicApp.Models;

// обрав варіант Б, бо можна задати свій коефіцієнт при створенні запису і не робити окремі віп-класи
public class RegularAppointment : Appointment {
    private readonly decimal _multiplier;

    public RegularAppointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30, decimal multiplier = 1m)
        : base(patientId, doctorId, scheduledAt, durationMinutes) {
        _multiplier = multiplier;
    }

    public override decimal GetCost() {
        return base.GetCost() * _multiplier;
    }

    public override string GetDescription() {
        return "Звичайний прийом";
    }
}
