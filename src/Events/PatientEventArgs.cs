namespace ClinicApp.Events;

public class PatientEventArgs : EventArgs {
    public int PatientId { get; }
    public string FullName { get; }

    public PatientEventArgs(int patientId, string fullName) {
        PatientId = patientId;
        FullName = fullName;
    }
}
