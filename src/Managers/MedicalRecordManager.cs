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
}
