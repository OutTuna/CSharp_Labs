using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;
    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public int Id { get; }
    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public Speciality Speciality { get; set; }
    public string LicenseNumber
    {
        get => _licenseNumber;
        set => _licenseNumber = value;
    }

    public string Phone
    {
        get => _phone;
        set => _phone = value;
    }

    public WorkSchedule Schedule { get; set; }
    public string FullName => FirstName + " " + LastName;
    public int WorkingHoursPerDay => Schedule.HoursPerDay;
    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor() : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(
        string firstName,
        string lastName,
        Speciality speciality,
        string licenseNumber,
        string phone)
    {
        Id = _nextId++;

        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;

        Schedule = new WorkSchedule(8, 17);
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        string specialityText = ClinicFormatter.FormatSpeciality(Speciality);
        string phoneText = ClinicFormatter.FormatPhone(Phone);

        return $"[{Id}] {FullName} | {specialityText} | {LicenseNumber} | Тел: {phoneText} | {Schedule} | {status}";
    }
}