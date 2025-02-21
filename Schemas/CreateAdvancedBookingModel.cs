namespace CompClubAPI.Schemas;

public record CreateAdvancedBookingModel(
    int IdWorkingSpace,
    DateTime StartTime,
    DateTime EndTime
    );