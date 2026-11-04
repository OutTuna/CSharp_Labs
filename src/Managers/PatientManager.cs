using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private readonly List<Patient> _patients = new List<Patient>();
    public int Count => _patients.Count;

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= Count) {
                return null;
            }

            return _patients[index];
        }
    }

    public void Add(Patient patient) {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id) {
        for (int i = 0; i < Count; i++) {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public bool TryFindById(int id, out Patient patient) {
        Patient? found = FindById(id);

        if (found == null) {
            patient = null!;
            return false;
        }

        patient = found;
        return true;
    }

    public Patient[] FindByName(string query) {
        string trimmedQuery = query.Trim();

        if (trimmedQuery.Length == 0) {
            return new Patient[0];
        }

        string queryLower = trimmedQuery.ToLower();

        List<Patient> result = new List<Patient>();

        for (int i = 0; i < Count; i++) {
            string firstNameLower = _patients[i].FirstName.ToLower();
            string lastNameLower = _patients[i].LastName.ToLower();

            if (firstNameLower.Contains(queryLower) || lastNameLower.Contains(queryLower)) {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType) {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < Count; i++) {
            if (_patients[i].BloodType == bloodType) {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] GetAll() {
        return _patients.ToArray();
    }

    public bool Remove(int id) {
        for (int i = 0; i < Count; i++) {
            if (_patients[i].Id == id) {
                _patients.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void DisplayAll() {
        if (Count == 0) {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"Пацієнти ({Count})");

        for (int i = 0; i < Count; i++) {
            Console.WriteLine(_patients[i]);
        }
    }
    public void DisplayStats() {
        if (Count == 0) {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        int totalAge = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < Count; i++) {
            totalAge += _patients[i].Age;

            if (_patients[i].Age < _patients[youngestIndex].Age) {
                youngestIndex = i;
            }

            if (_patients[i].Age > _patients[oldestIndex].Age) {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult) {
                adultCount++;
            }
        }

        double averageAge = (double)totalAge / Count;
        
        Console.WriteLine($"Всього:       {Count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultCount} з {Count}");
    }
}