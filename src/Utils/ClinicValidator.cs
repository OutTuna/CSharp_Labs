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
        if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10) {
            throw new ArgumentException("Телефон має містити рівно 10 цифр.", nameof(phone));
        }

        for (int i = 0; i < phone.Length; i++) {
            if (phone[i] < '0' || phone[i] > '9') {
                throw new ArgumentException("Телефон має містити лише цифри 0–9.", nameof(phone));
            }
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
