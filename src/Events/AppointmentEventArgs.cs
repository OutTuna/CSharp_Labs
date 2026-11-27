namespace ClinicApp.Events;

public class AppointmentEventArgs : EventArgs {
    public int AppointmentId { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; }
    public string Notes { get; }

    public AppointmentEventArgs(int appointmentId, int patientId, int doctorId, DateTime scheduledAt, string notes = "") {
        AppointmentId = appointmentId;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        Notes = notes;
    }
}
