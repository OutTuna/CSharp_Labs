using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager {
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients) {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats() {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < doctors.Length; i++) {
            int count = 0;
            decimal revenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++) {
                if (appointments[j].DoctorId == doctors[i].Id) {
                    count++;
                    revenue += appointments[j].GetCost();

                    if (appointments[j].ScheduledAt > lastDate) {
                        lastDate = appointments[j].ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(doctors[i].Id, doctors[i].FullName, count, revenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats() {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < patients.Length; i++) {
            int count = 0;
            decimal spent = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++) {
                if (appointments[j].PatientId == patients[i].Id) {
                    count++;
                    spent += appointments[j].GetCost();

                    if (appointments[j].ScheduledAt > lastDate) {
                        lastDate = appointments[j].ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(patients[i].Id, patients[i].FullName, count, spent, lastDate);
        }
    }
}
