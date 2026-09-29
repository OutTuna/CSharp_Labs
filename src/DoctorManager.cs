namespace ClinicApp;

public class DoctorManager {
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;
    public int Count => _count;

    public Doctor? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) {
                return null;
            }

            return _doctors[index];
        }
    }

    public void Add(Doctor doctor) {
        if (_count == MaxDoctors) {
            Console.WriteLine("Ліміт лікарів вичерпано.");
            return;
        }

        _doctors[_count] = doctor;
        _count++;

        Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
    }

    public Doctor? FindById(int id) {
        for (int i = 0; i < _count; i++) {
            if (_doctors[i].Id == id) {
                return _doctors[i];
            }
        }

        return null;
    }

    public bool TryFindById(int id, out Doctor doctor) {
        Doctor? found = FindById(id);

        if (found == null) {
            doctor = null!;
            return false;
        }

        doctor = found;
        return true;
    }

    // Пошук за частиною рядка (як у Лабі 03)
    public Doctor[] FindBySpeciality(string query) {
        string trimmedQuery = query.Trim();

        if (trimmedQuery.Length == 0) {
            return new Doctor[0];
        }

        string queryLower = trimmedQuery.ToLower();

        int matches = 0;

        for (int i = 0; i < _count; i++) {
            string specialityLower = _doctors[i].Speciality.ToString().ToLower();

            if (specialityLower.Contains(queryLower)) {
                matches++;
            }
        }

        Doctor[] result = new Doctor[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            string specialityLower = _doctors[i].Speciality.ToString().ToLower();

            if (specialityLower.Contains(queryLower)) {
                result[index] = _doctors[i];
                index++;
            }
        }

        return result;
    }

    // Точний збіг за enum
    public Doctor[] FindBySpeciality(Speciality speciality) {
        int matches = 0;

        for (int i = 0; i < _count; i++) {
            if (_doctors[i].Speciality == speciality) {
                matches++;
            }
        }

        Doctor[] result = new Doctor[matches];

        int index = 0;

        for (int i = 0; i < _count; i++) {
            if (_doctors[i].Speciality == speciality) {
                result[index] = _doctors[i];
                index++;
            }
        }

        return result;
    }

    public Doctor[] GetAll() {
        Doctor[] result = new Doctor[_count];

        for (int i = 0; i < _count; i++) {
            result[i] = _doctors[i];
        }

        return result;
    }

    public bool Remove(int id) {
        int indexToRemove = -1;

        for (int i = 0; i < _count; i++) {
            if (_doctors[i].Id == id) {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) {
            return false;
        }

        for (int i = indexToRemove; i < _count - 1; i++) {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll() {
        if (_count == 0) {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"Лікарі ({_count} / {MaxDoctors})");

        for (int i = 0; i < _count; i++) {
            Console.WriteLine(_doctors[i]);
        }
    }

    public void DisplayStats() {
        if (_count == 0) {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        int availableNow = 0;

        for (int i = 0; i < _count; i++) {
            if (_doctors[i].IsAvailableNow) {
                availableNow++;
            }
        }

        Console.WriteLine($"Всього:         {_count}");
        Console.WriteLine($"Доступні зараз: {availableNow}");
        Console.WriteLine("По спеціальностях:");

        for (int i = 0; i < _count; i++) {
            bool alreadySeen = false;

            for (int j = 0; j < i; j++) {
                if (_doctors[i].Speciality == _doctors[j].Speciality) {
                    alreadySeen = true;
                    break;
                }
            }

            if (!alreadySeen) {
                int specialityCount = 0;

                for (int k = 0; k < _count; k++) {
                    if (_doctors[k].Speciality == _doctors[i].Speciality) {
                        specialityCount++;
                    }
                }
                Console.WriteLine($"  {ClinicFormatter.FormatSpeciality(_doctors[i].Speciality)}: {specialityCount}");
            }
        }
    }
}