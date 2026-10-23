namespace ClinicApp.Interfaces;

public interface ISchedulable {
    bool CanSchedule(DateTime at);
    DateTime[] GetAvailableSlots(DateTime date, int slotCount);
}
