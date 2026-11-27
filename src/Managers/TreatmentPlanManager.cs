using ClinicApp.Enums;
using ClinicApp.Events;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers;

public class TreatmentPlanManager {
    private readonly List<TreatmentPlan> _plans = new List<TreatmentPlan>();

    public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

    public bool Add(TreatmentPlan plan) {
        ValidationResult validation = ModelValidator.Validate(plan);

        if (!validation.IsValid) {
            Console.WriteLine("План не додано:");
            validation.Print();
            return false;
        }

        _plans.Add(plan);
        Console.WriteLine($"План [{plan.Id}] додано.");
        return true;
    }

    public bool Complete(int planId) {
        TreatmentPlan? plan = GetById(planId);

        if (plan == null || !plan.Complete()) {
            return false;
        }

        PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(plan.Id, plan.PatientId, plan.Diagnosis));
        return true;
    }

    public TreatmentPlan? GetById(int id) {
        for (int i = 0; i < _plans.Count; i++) {
            if (_plans[i].Id == id) {
                return _plans[i];
            }
        }

        return null;
    }

    public TreatmentPlan[] GetByPatient(int patientId) {
        List<TreatmentPlan> result = new List<TreatmentPlan>();

        for (int i = 0; i < _plans.Count; i++) {
            if (_plans[i].PatientId == patientId) {
                result.Add(_plans[i]);
            }
        }

        return result.ToArray();
    }

    public TreatmentPlan[] GetByStatus(TreatmentStatus status) {
        List<TreatmentPlan> result = new List<TreatmentPlan>();

        for (int i = 0; i < _plans.Count; i++) {
            if (_plans[i].Status == status) {
                result.Add(_plans[i]);
            }
        }

        return result.ToArray();
    }

    public TreatmentPlan[] GetAll() {
        return _plans.ToArray();
    }
}
