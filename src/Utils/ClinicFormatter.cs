using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter {
    public static string FormatBloodType(BloodType bt) {
        switch (bt) {
            case BloodType.APositive: return "A+";
            case BloodType.ANegative: return "A-";
            case BloodType.BPositive: return "B+";
            case BloodType.BNegative: return "B-";
            case BloodType.ABPositive: return "AB+";
            case BloodType.ABNegative: return "AB-";
            case BloodType.OPositive: return "O+";
            case BloodType.ONegative: return "O-";
            default: return "Невідомо";
        }
    }

    public static string FormatSpeciality(Speciality s) {
        switch (s) {
            case Speciality.General: return "Загальна практика";
            case Speciality.Cardiology: return "Кардіологія";
            case Speciality.Neurology: return "Неврологія";
            case Speciality.Pediatrics: return "Педіатрія";
            case Speciality.Surgery: return "Хірургія";
            case Speciality.Orthopedics: return "Ортопедія";
            case Speciality.Dermatology: return "Дерматологія";
            case Speciality.Emergency: return "Невідкладна допомога";
            default: return "Невідомо";
        }
    }

    public static string FormatAge(int age) {
        int mod100 = age % 100;

        if (mod100 >= 11 && mod100 <= 19) {
            return age + " років";
        }

        int mod10 = age % 10;

        if (mod10 == 1) {
            return age + " рік";
        }

        if (mod10 == 2 || mod10 == 3 || mod10 == 4) {
            return age + " роки";
        }

        return age + " років";
    }

    public static string FormatPhone(string phone) {
        if (phone.Length != 10) {
            return phone;
        }

        for (int i = 0; i < phone.Length; i++) {
            if (!char.IsDigit(phone[i])) {
                return phone;
            }
        }

        return "(" + phone.Substring(0, 3) + ") "
            + phone.Substring(3, 3) + "-"
            + phone.Substring(6);
    }
}