using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;
    private string _firstName = "";
    private string _lastName = "";
    private DateTime _dateOfBirth;
    private string _phone = "";


    public int Id { get; }
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50) {
                throw new ArgumentException("Ім'я має містити від 1 до 50 символів і не бути порожнім.", nameof(FirstName));
            }

            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50) {
                throw new ArgumentException("Ім'я має містити від 1 до 50 символів і не бути порожнім.", nameof(LastName));
            }

            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today) {
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата не може бути в майбутньому.");
            }

            if (value.Year < 1900) {
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата не може бути раніше 1900 року.");
            }

            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }
    public string Phone
    {
        get => _phone;
        set
        {
            if (value == null || value.Length != 10) {
                throw new ArgumentException("Телефон має містити рівно 10 цифр.", nameof(Phone));
            }

            for (int i = 0; i < value.Length; i++) {
                if (value[i] < '0' || value[i] > '9') {
                    throw new ArgumentException("Телефон має містити лише цифри 0–9.", nameof(Phone));
                }
            }

            _phone = value;
        }
    }

    public string Email { get; set; }

    public string FullName => FirstName + " " + LastName;

    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
    }

    public bool IsAdult => Age >= 18;

    public Patient() : this("Невідомий", "Пацієнт") { }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000") { }

    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
        Id = _nextId++;
    }

    public string GetAgeCategory()
    {
        if (Age < 18) return "дитина";
        if (Age < 60) return "дорослий";
        return "літній";
    }

    public override string ToString()
    {
        string blood = ClinicFormatter.FormatBloodType(BloodType);
        string ageText = ClinicFormatter.FormatAge(Age);
        string phoneText = ClinicFormatter.FormatPhone(Phone);

        return $"[{Id}] {FullName} | Вік: {ageText} ({GetAgeCategory()}) | Кров: {blood} | Тел: {phoneText}";
    }
}