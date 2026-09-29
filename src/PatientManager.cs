namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;
    public int Count => _count;

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) {
                return null;
            }

            return _patients[index];
        }
    }

    public void Add(Patient patient) {
        if (_count == MaxPatients) {
            Console.WriteLine("Ліміт пацієнтів вичерпано.");
            return;
        }
        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id) {
        for (int i = 0; i < _count; i++) {
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

        int matches = 0;

        for (int i = 0; i < _count; i++) {
            string firstNameLower = _patients[i].FirstName.ToLower();
            string lastNameLower = _patients[i].LastName.ToLower();

            if (firstNameLower.Contains(queryLower) || lastNameLower.Contains(queryLower)) {
                matches++;
            }
        }

        Patient[] result = new Patient[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            string firstNameLower = _patients[i].FirstName.ToLower();
            string lastNameLower = _patients[i].LastName.ToLower();

            if (firstNameLower.Contains(queryLower) || lastNameLower.Contains(queryLower)) {
                result[index] = _patients[i];
                index++;
            }
        }

        return result;
    }

    public Patient[] FindByBloodType(BloodType bloodType) {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_patients[i].BloodType == bloodType) {
                matches++;
            }
        }

        Patient[] result = new Patient[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_patients[i].BloodType == bloodType) {
                result[index] = _patients[i];
                index++;
            }
        }

        return result;
    }

    public bool Remove(int id) {
        int indexToRemove = -1;
        
        for (int i = 0; i < _count; i++) {
            if (_patients[i].Id == id) {
                indexToRemove = i;
                break;
            }
        }
        
        if (indexToRemove == -1) {
            return false;
        }
        
        for (int i = indexToRemove; i < _count - 1; i++) {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll() {
        if (_count == 0) {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"Пацієнти ({_count} / {MaxPatients})");

        for (int i = 0; i < _count; i++) {
            Console.WriteLine(_patients[i]);
        }
    }
    public void DisplayStats() {
        if (_count == 0) {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        int totalAge = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < _count; i++) {
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

        double averageAge = (double)totalAge / _count;
        
        Console.WriteLine($"Всього:       {_count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultCount} з {_count}");
    }
}