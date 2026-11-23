namespace ClinicApp.Utils;

public class ImportResult {
    private readonly List<string> _errors = new List<string>();

    public int Imported { get; private set; }
    public int Skipped { get; private set; }
    public IReadOnlyList<string> Errors => _errors;

    public void AddSuccess() {
        Imported++;
    }

    public void AddError(int lineNumber, string reason) {
        Skipped++;
        _errors.Add($"Рядок {lineNumber}: {reason}");
    }

    public void Print() {
        Console.WriteLine($"Імпортовано: {Imported} | Пропущено: {Skipped}");

        foreach (string error in _errors) {
            Console.WriteLine($"  {error}");
        }
    }
}
