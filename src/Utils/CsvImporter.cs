using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public static class CsvImporter {
    private const int FieldCount = 5;

    public static ImportResult ImportPatients(Clinic clinic, string filePath) {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath)) {
            result.AddError(0, $"Файл не знайдено: {filePath}");
            return result;
        }

        string[] lines;

        try {
            lines = File.ReadAllLines(filePath, Encoding.UTF8);
        }
        catch (IOException e) {
            result.AddError(0, e.Message);
            return result;
        }
        catch (UnauthorizedAccessException e) {
            result.AddError(0, e.Message);
            return result;
        }

        for (int i = 1; i < lines.Length; i++) {
            string line = lines[i].Trim();

            if (line.Length == 0) {
                continue;
            }

            int lineNumber = i + 1;

            try {
                Patient patient = ParsePatient(line);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (FormatException e) {
                result.AddError(lineNumber, e.Message);
            }
            catch (ArgumentException e) {
                result.AddError(lineNumber, e.Message);
            }
        }

        return result;
    }

    private static Patient ParsePatient(string line) {
        string[] parts = line.Split(',');

        if (parts.Length != FieldCount) {
            throw new FormatException($"Рядок має {parts.Length} {FieldsWord(parts.Length)} замість {FieldCount}.");
        }

        string firstName = parts[0].Trim();
        string lastName = parts[1].Trim();

        if (firstName.Length == 0) {
            throw new FormatException("Ім'я не може бути порожнім.");
        }

        if (lastName.Length == 0) {
            throw new FormatException("Прізвище не може бути порожнім.");
        }

        DateTime dateOfBirth = ParseDate(parts[2].Trim());
        BloodType bloodType = ParseBloodType(parts[3].Trim());

        return new Patient(firstName, lastName, dateOfBirth, bloodType, parts[4].Trim());
    }

    private static DateTime ParseDate(string text) {
        try {
            return DateTime.ParseExact(text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
        catch (FormatException) {
            throw new FormatException($"Некоректна дата «{text}», очікується дд.мм.рррр.");
        }
    }

    private static BloodType ParseBloodType(string text) {
        try {
            BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), text);

            if (bloodType.ToString() == text) {
                return bloodType;
            }
        }
        catch (ArgumentException) {
        }
        catch (OverflowException) {
        }

        throw new FormatException($"Невідома група крові «{text}».");
    }

    private static string FieldsWord(int count) {
        int mod100 = count % 100;
        int mod10 = count % 10;

        if (mod100 >= 11 && mod100 <= 19) {
            return "полів";
        }

        if (mod10 == 1) {
            return "поле";
        }

        if (mod10 >= 2 && mod10 <= 4) {
            return "поля";
        }

        return "полів";
    }
}
