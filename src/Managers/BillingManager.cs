using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class BillingManager {
    private AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments) {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid() {
        var appointments = _appointments.GetAll();
        int count = 0;

        for (int i = 0; i < appointments.Length; i++) {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled) {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int index = 0;

        for (int i = 0; i < appointments.Length; i++) {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled) {
                result[index] = appointments[i];
                index++;
            }
        }

        return result;
    }

    public IPayable[] GetUnpaidByPatient(int patientId) {
        var appointments = _appointments.GetByPatient(patientId);
        int count = 0;

        for (int i = 0; i < appointments.Length; i++) {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled) {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int index = 0;

        for (int i = 0; i < appointments.Length; i++) {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled) {
                result[index] = appointments[i];
                index++;
            }
        }

        return result;
    }

    public decimal GetTotalDebt() {
        IPayable[] items = GetAllUnpaid();
        decimal total = 0m;

        for (int i = 0; i < items.Length; i++) {
            total += items[i].GetCost();
        }

        return total;
    }

    public decimal GetPatientDebt(int patientId) {
        IPayable[] items = GetUnpaidByPatient(patientId);
        decimal total = 0m;

        for (int i = 0; i < items.Length; i++) {
            total += items[i].GetCost();
        }

        return total;
    }

    public bool PayAppointment(int appointmentId) {
        var appointments = _appointments.GetAll();

        for (int i = 0; i < appointments.Length; i++) {
            if (appointments[i].Id == appointmentId) {
                if (appointments[i].IsPaid || appointments[i].IsCancelled) {
                    return false;
                }

                appointments[i].MarkPaid();
                return appointments[i].IsPaid;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items) {
        if (items.Length == 0) {
            Console.WriteLine("Неоплачених записів немає.");
            return;
        }

        for (int i = 0; i < items.Length; i++) {
            Console.WriteLine($"{items[i]} | Сума: {items[i].GetCost():F2}");
        }
    }
}
