using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager {
    private const string PatientsSection = "[PATIENTS]";
    private const string EndSection = "[END]";
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat") {
        _sessionPath = sessionPath;
    }

    public bool Exists() {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic) {
        Patient[] patients = clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine(PatientsSection);

        foreach (Patient p in patients) {
            writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
        }

        writer.WriteLine(EndSection);
    }

    public int Load(Clinic clinic) {
        if (!Exists()) {
            return 0;
        }

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string section = "";
        int added = 0;

        foreach (string rawLine in lines) {
            string line = rawLine.Trim();

            if (line.Length == 0) {
                continue;
            }

            if (line.StartsWith("[")) {
                section = line;
                continue;
            }

            if (section != PatientsSection) {
                continue;
            }

            try {
                string[] parts = line.Split(',');

                if (parts.Length != 5) {
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                DateTime dateOfBirth = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());

                if (IsKnown(clinic, firstName, lastName, dateOfBirth)) {
                    continue;
                }

                clinic.Patients.Add(new Patient(firstName, lastName, dateOfBirth, bloodType, parts[4].Trim()));
                added++;
            }
            catch (FormatException) {
            }
            catch (ArgumentException) {
            }
            catch (OverflowException) {
            }
        }

        return added;
    }

    private static bool IsKnown(Clinic clinic, string firstName, string lastName, DateTime dateOfBirth) {
        Patient[] patients = clinic.Patients.GetAll();

        foreach (Patient p in patients) {
            if (p.FirstName == firstName && p.LastName == lastName && p.DateOfBirth.Date == dateOfBirth.Date) {
                return true;
            }
        }

        return false;
    }
}
