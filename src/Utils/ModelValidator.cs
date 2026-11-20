using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator {
    public static ValidationResult Validate(object obj) {
        ValidationResult result = new ValidationResult();
        PropertyInfo[] properties = obj.GetType().GetProperties();

        foreach (PropertyInfo prop in properties) {
            object? value = prop.GetValue(obj);

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null) {
                if (value == null || (value is string text && string.IsNullOrWhiteSpace(text))) {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string str && str.Length > maxLength.Length) {
                string message = maxLength.ErrorMessage.Length > 0 ? maxLength.ErrorMessage : $"довжина перевищує {maxLength.Length}.";
                result.AddError($"{prop.Name}: {message}");
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null && value != null) {
                string message = minValue.ErrorMessage.Length > 0 ? minValue.ErrorMessage : $"значення менше за {minValue.Min}.";

                try {
                    if (Convert.ToDouble(value) < minValue.Min) {
                        result.AddError($"{prop.Name}: {message}");
                    }
                }
                catch (FormatException) {
                    result.AddError($"{prop.Name}: {message}");
                }
                catch (InvalidCastException) {
                    result.AddError($"{prop.Name}: {message}");
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type) {
        Console.WriteLine(type.Name);

        PropertyInfo[] properties = type.GetProperties();
        int width = 0;

        foreach (PropertyInfo prop in properties) {
            if (prop.Name.Length > width) {
                width = prop.Name.Length;
            }
        }

        foreach (PropertyInfo prop in properties) {
            string line = "  " + prop.Name.PadRight(width + 2);

            foreach (Attribute attribute in prop.GetCustomAttributes()) {
                string name = attribute.GetType().Name;

                if (name.EndsWith("Attribute")) {
                    name = name.Substring(0, name.Length - "Attribute".Length);
                }

                line += $"[{name}] ";
            }

            Console.WriteLine(line.TrimEnd());
        }
    }
}
