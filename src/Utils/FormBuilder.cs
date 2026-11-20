using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder {
    public static T Build<T>() where T : new() {
        T obj = new T();
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo prop in properties) {
            if (prop.GetSetMethod() == null) {
                continue;
            }

            string prompt = BuildPrompt(prop);

            while (true) {
                Console.Write($"  {prompt}: ");
                string? input = Console.ReadLine();

                if (input == null) {
                    return obj;
                }

                try {
                    object converted = Convert.ChangeType(input.Trim(), prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch (FormatException) {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
                catch (InvalidCastException) {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
                catch (OverflowException) {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    private static string BuildPrompt(PropertyInfo prop) {
        List<string> hints = new List<string>();

        if (prop.GetCustomAttribute<RequiredAttribute>() != null) {
            hints.Add("обов'язкове");
        }

        MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLength != null) {
            hints.Add($"макс. {maxLength.Length}");
        }

        MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
        if (minValue != null) {
            hints.Add($"мін. {minValue.Min}");
        }

        if (hints.Count == 0) {
            return prop.Name;
        }

        return $"{prop.Name} ({string.Join(", ", hints)})";
    }
}
