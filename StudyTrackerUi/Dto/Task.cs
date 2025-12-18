namespace StudyTrackerUi.Dto;

public record Task(
    Guid Id,
    Guid OwnerId,
    Guid? ParentId,
    DateOnly BeginDate,
    DateOnly EndDate,
    string Name,
    string? Description,
    bool? Success);