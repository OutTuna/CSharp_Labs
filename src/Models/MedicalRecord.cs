using ClinicApp.Utils;

namespace ClinicApp.Models;

public abstract class MedicalRecord
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime Date { get; }
    public string Notes { get; set; } = "";

    protected MedicalRecord(int patientId, int doctorId, DateTime date)
    {
        ClinicValidator.ValidatePositive(patientId, nameof(patientId));
        ClinicValidator.ValidatePositive(doctorId, nameof(doctorId));
        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Id = _nextId++;
    }

    public abstract string GetSummary();

    public virtual string GetRecordType() => "Медичний запис";

    public virtual bool IsActive() => Date >= DateTime.Today.AddMonths(-6);

    public override string ToString()
    {
        string result = $"[{Id}] {GetRecordType()} | {Date:dd.MM.yyyy} | {GetSummary()}";
        return string.IsNullOrWhiteSpace(Notes) ? result : $"{result} | {Notes}";
    }
}
