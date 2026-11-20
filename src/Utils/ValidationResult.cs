namespace ClinicApp.Utils;

public class ValidationResult {
    private readonly List<string> _errors = new List<string>();

    public bool IsValid => _errors.Count == 0;
    public IReadOnlyList<string> Errors => _errors;

    public void AddError(string error) {
        _errors.Add(error);
    }

    public void Print() {
        foreach (string error in _errors) {
            Console.WriteLine($"  [!] {error}");
        }
    }
}
