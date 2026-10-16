using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50) {
            throw new ArgumentException("Ім'я має містити від 1 до 50 символів і не бути порожнім.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (phone == null || !Regex.IsMatch(phone, @"^[0-9]{10}\z")) {
            throw new ArgumentException("Телефон має містити рівно 10 цифр 0–9.", nameof(phone));
        }
    }

    public static void ValidateEmail(string email)
    {
        if (email == "") {
            return;
        }

        if (email == null || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s.]+\.[^@\s.]+(\.[^@\s.]+)*\z")) {
            throw new ArgumentException("Некоректний формат email.", nameof(email));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today) {
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому.");
        }

        if (value.Year < 1900) {
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути раніше 1900 року.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0) {
            throw new ArgumentOutOfRangeException(fieldName, "Тривалість має бути більшою за нуль.");
        }
    }
}
