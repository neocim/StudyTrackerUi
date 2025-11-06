namespace StudyTrackerUi.Api.Dto;

public record Task(
    Guid Id,
    Guid OwnerId,
    Guid? ParentId,
    ICollection<Task>? SubTasks,
    DateOnly BeginDate,
    DateOnly EndDate,
    string Name,
    string? Description,
    bool? Success);