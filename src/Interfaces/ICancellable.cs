namespace ClinicApp.Interfaces;

public interface ICancellable {
    bool IsCancelled { get; }
    string CancellationReason { get; }
    bool Cancel(string reason = "");
}
