using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Diagnosis : MedicalRecord
{
    private string _diagnosisCode = "";
    private string _description = "";

    public string DiagnosisCode
    {
        get => _diagnosisCode;
        set
        {
            ClinicValidator.ValidateName(value, nameof(DiagnosisCode));
            _diagnosisCode = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Description));
            _description = value;
        }
    }

    public bool IsChronic { get; set; }

    public Diagnosis(int patientId, int doctorId, DateTime date, string diagnosisCode,
        string description, bool isChronic = false) : base(patientId, doctorId, date)
    {
        DiagnosisCode = diagnosisCode;
        Description = description;
        IsChronic = isChronic;
    }

    public override string GetSummary() => $"{DiagnosisCode}: {Description}" + (IsChronic ? " [хронічне]" : "");

    public override string GetRecordType() => "Діагноз";
}
