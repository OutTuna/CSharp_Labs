namespace ClinicApp.Models;

public struct WorkSchedule {
    public int Start { get; }
    public int End { get; }

    public WorkSchedule(int start, int end) {
        if (start < 0 || start > 23) {
            throw new ArgumentOutOfRangeException(nameof(start), "Початок роботи має бути від 0 до 23.");
        }

        if (end < 1 || end > 24) {
            throw new ArgumentOutOfRangeException(nameof(end), "Кінець роботи має бути від 1 до 24.");
        }

        if (start >= end) {
            throw new ArgumentException("Початок роботи має бути раніше за кінець.", nameof(start));
        }

        Start = start;
        End = end;
    }

    public int HoursPerDay => End - Start;

    public string Display => Start.ToString("D2") + ":00–" + End.ToString("D2") + ":00";

    public bool IsNow => Contains(DateTime.Now.Hour);

    public bool Contains(int hour) {
        return hour >= Start && hour < End;
    }

    public override string ToString() {
        return Display + " (" + HoursPerDay + " год)";
    }
}