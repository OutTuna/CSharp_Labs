using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter {
    private const string Separator = "----------------------------------------";
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports") {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    public string ExportPatients() {
        Patient[] patients = _clinic.Patients.GetAll();
        string path = Path.Combine(GetReportDir(), "patients.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Пацієнти");

        for (int i = 0; i < patients.Length; i++) {
            Patient p = patients[i];
            string blood = ClinicFormatter.FormatBloodType(p.BloodType);
            string phone = ClinicFormatter.FormatPhone(p.Phone);
            writer.WriteLine($"{i + 1,3}. [{p.Id}] {p.FullName} | {ClinicFormatter.FormatAge(p.Age)} | {blood} | {phone}");
        }

        writer.WriteLine(Separator);
        writer.WriteLine($"Всього: {patients.Length}");

        return path;
    }

    public string ExportAppointments() {
        Appointment[] appointments = _clinic.Appointments.GetAll();
        string path = Path.Combine(GetReportDir(), "appointments.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Записи на прийом");

        for (int i = 0; i < appointments.Length; i++) {
            writer.WriteLine($"{i + 1,3}. {appointments[i]}");
        }

        writer.WriteLine(Separator);
        writer.WriteLine($"Всього: {appointments.Length}");

        return path;
    }

    public string ExportBilling() {
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        string path = Path.Combine(GetReportDir(), "billing.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Неоплачені записи");

        for (int i = 0; i < unpaid.Length; i++) {
            writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
        }

        writer.WriteLine(Separator);
        writer.WriteLine($"Всього: {unpaid.Length}");
        writer.WriteLine($"Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн");

        return path;
    }

    public string ExportTreatmentPlans() {
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();
        string path = Path.Combine(GetReportDir(), "treatment_plans.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Плани лікування");

        for (int i = 0; i < plans.Length; i++) {
            writer.WriteLine($"{i + 1,3}. {plans[i]}");
        }

        writer.WriteLine(Separator);
        writer.WriteLine($"Всього: {plans.Length}");

        return path;
    }

    public string[] ExportAll() {
        return new string[] {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }

    private string GetReportDir() {
        string dir = Path.Combine(_baseDir, DateTime.Today.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static void WriteHeader(StreamWriter writer, string title) {
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(Separator);
    }
}
