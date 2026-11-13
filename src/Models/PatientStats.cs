namespace ClinicApp.Models;

public class PatientStats : IComparable<PatientStats> {
    public int PatientId { get; }
    public string FullName { get; }
    public int VisitCount { get; }
    public decimal TotalSpent { get; }
    public DateTime LastVisitDate { get; }

    public PatientStats(int patientId, string fullName, int visitCount, decimal totalSpent, DateTime lastVisitDate) {
        PatientId = patientId;
        FullName = fullName;
        VisitCount = visitCount;
        TotalSpent = totalSpent;
        LastVisitDate = lastVisitDate;
    }

    public int CompareTo(PatientStats? other) {
        if (other == null) {
            return -1;
        }

        return other.VisitCount.CompareTo(VisitCount);
    }

    public override string ToString() {
        string lastDate = LastVisitDate == DateTime.MinValue ? "—" : $"{LastVisitDate:dd.MM.yyyy}";

        return $"[{PatientId}] {FullName} | Візитів: {VisitCount} | Витрачено: {TotalSpent:F2} грн | Останній візит: {lastDate}";
    }
}
