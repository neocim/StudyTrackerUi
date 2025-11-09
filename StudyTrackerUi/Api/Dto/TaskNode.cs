namespace StudyTrackerUi.Api.Dto;

public record TaskNode(
    Guid Id,
    Guid OwnerId,
    Guid? ParentId,
    ICollection<TaskNode>? SubTasks,
    DateOnly BeginDate,
    DateOnly EndDate,
    string Name,
    string? Description,
    bool? Success);