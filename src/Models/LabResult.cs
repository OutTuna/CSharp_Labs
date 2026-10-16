using ClinicApp.Utils;

namespace ClinicApp.Models;

public class LabResult : MedicalRecord
{
    private string _testName = "";
    private string _unit = "";
    private string _referenceRange = "";

    public string TestName
    {
        get => _testName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(TestName));
            _testName = value;
        }
    }

    public string Unit
    {
        get => _unit;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Unit));
            _unit = value;
        }
    }

    public string ReferenceRange
    {
        get => _referenceRange;
        set
        {
            ClinicValidator.ValidateName(value, nameof(ReferenceRange));
            _referenceRange = value;
        }
    }

    public double Value { get; set; }
    public bool IsNormal { get; set; }

    public LabResult(int patientId, int doctorId, DateTime date, string testName,
        double value, string unit, string referenceRange, bool isNormal) : base(patientId, doctorId, date)
    {
        TestName = testName;
        Value = value;
        Unit = unit;
        ReferenceRange = referenceRange;
        IsNormal = isNormal;
    }

    public override string GetSummary() => $"{TestName}: {Value} {Unit} (норма: {ReferenceRange})"
        + (IsNormal ? "" : " ⚠ поза нормою");

    public override string GetRecordType() => "Аналіз";
}
