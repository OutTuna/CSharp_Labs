using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count;

    public int Count => _count;
    public MedicalRecord? this[int index] => index >= 0 && index < _count ? _records[index] : null;

    public void Add(MedicalRecord record)
    {
        if (_count == MaxRecords)
        {
            Console.WriteLine("Ліміт медичних записів вичерпано.");
            return;
        }

        _records[_count++] = record;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
                return _records[i];
        }

        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                count++;
        }

        MedicalRecord[] result = new MedicalRecord[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                result[index++] = _records[i];
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                count++;
        }

        MedicalRecord[] result = new MedicalRecord[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                result[index++] = _records[i];
        }

        return result;
    }

    public void DisplayAll()
    {
        MedicalRecord[] records = new MedicalRecord[_count];
        for (int i = 0; i < _count; i++)
            records[i] = _records[i];
        DisplayList(records);
    }

    public void DisplayList(MedicalRecord[] records)
    {
        switch (records.Length)
        {
            case 0:
                Console.WriteLine("Медичних записів немає.");
                return;
        }

        for (int i = 0; i < records.Length; i++)
            Console.WriteLine(records[i]);
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis record)
                count++;
        }

        Diagnosis[] result = new Diagnosis[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis record)
                result[index++] = record;
        }

        return result;
    }
    public LabResult[] GetLabResults(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult record)
                count++;
        }

        LabResult[] result = new LabResult[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult record)
                result[index++] = record;
        }

        return result;
    }
    public Prescription[] GetPrescriptions(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            Prescription? record = _records[i] as Prescription;
            if (record != null && record.PatientId == patientId)
                count++;
        }

        Prescription[] result = new Prescription[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            Prescription? record = _records[i] as Prescription;
            if (record != null && record.PatientId == patientId)
                result[index++] = record;
        }

        return result;
    }
    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis record && record.IsChronic)
                count++;
        }

        Diagnosis[] result = new Diagnosis[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis record && record.IsChronic)
                result[index++] = record;
        }

        return result;
    }
    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int count = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription record && record.IsActive())
                count++;
        }

        Prescription[] result = new Prescription[count];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription record && record.IsActive())
                result[index++] = record;
        }

        return result;
    }
    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] records = GetByPatient(patientId);
        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        if (records.Length == 0)
        {
            Console.WriteLine("Медичних записів немає.");
            return;
        }

        int diagnoses = 0;
        int results = 0;
        int prescriptions = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis)
                diagnoses++;
            else if (records[i] is LabResult)
                results++;
            else if (records[i] is Prescription)
                prescriptions++;
        }

        Console.WriteLine($"Всього записів: {records.Length} (діагнозів: {diagnoses}, аналізів: {results}, рецептів: {prescriptions})");
        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            for (int i = 0; i < chronic.Length; i++)
                Console.WriteLine($"  {chronic[i]}");
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"Активні рецепти ({active.Length}):");
            for (int i = 0; i < active.Length; i++)
                Console.WriteLine($"  {active[i]} | до {active[i].ExpiresAt:dd.MM.yyyy}");
        }
    }
}
