using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter {
    private readonly Clinic _clinic;
    private readonly string _passportDir;

    public PatientPassportWriter(Clinic clinic, string passportDir = "patients") {
        _clinic = clinic;
        _passportDir = passportDir;
        Directory.CreateDirectory(_passportDir);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => WritePassport(e.PatientId);

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => WritePassport(e.PatientId);

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => WritePassport(e.PatientId);

    private void WritePassport(int patientId) {
        Patient? patient = _clinic.Patients.FindById(patientId);

        if (patient == null) {
            return;
        }

        string path = Path.Combine(_passportDir, $"passport_{patientId}.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine();

        string blood = ClinicFormatter.FormatBloodType(patient.BloodType);
        string phone = ClinicFormatter.FormatPhone(patient.Phone);
        writer.WriteLine($"Ім'я: {patient.FullName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
        writer.WriteLine($"Група крові: {blood}   Телефон: {phone}");
        writer.WriteLine();

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        writer.WriteLine("-- Діагнози --");
        int diagnoses = 0;
        foreach (MedicalRecord record in records) {
            if (record is Diagnosis diagnosis) {
                writer.WriteLine(diagnosis.GetSummary());
                diagnoses++;
            }
        }
        WriteEmptyMark(writer, diagnoses);

        writer.WriteLine("-- Аналізи --");
        int labResults = 0;
        foreach (MedicalRecord record in records) {
            if (record is LabResult labResult) {
                writer.WriteLine(labResult.GetSummary());
                labResults++;
            }
        }
        WriteEmptyMark(writer, labResults);

        writer.WriteLine("-- Рецепти --");
        int prescriptions = 0;
        foreach (MedicalRecord record in records) {
            if (record is Prescription prescription) {
                writer.WriteLine(prescription.GetSummary());
                prescriptions++;
            }
        }
        WriteEmptyMark(writer, prescriptions);
        writer.WriteLine();

        writer.WriteLine("-- Записи --");
        Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
        foreach (Appointment appointment in appointments) {
            writer.WriteLine($"[{appointment.Id}] {appointment.GetDescription()} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm} | {appointment.Status} | {appointment.GetCost():F2} грн");
        }
        WriteEmptyMark(writer, appointments.Length);
        writer.WriteLine();

        writer.WriteLine("-- Плани лікування --");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
        foreach (TreatmentPlan plan in plans) {
            writer.WriteLine($"[{plan.Id}] {plan.Diagnosis} | {plan.DurationDays} днів | {plan.Status}");
        }
        WriteEmptyMark(writer, plans.Length);
        writer.WriteLine();

        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
    }

    private static void WriteEmptyMark(StreamWriter writer, int count) {
        if (count == 0) {
            writer.WriteLine("немає");
        }
    }
}
