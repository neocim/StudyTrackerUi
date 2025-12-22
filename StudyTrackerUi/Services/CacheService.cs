using Microsoft.Extensions.Caching.Memory;
using StudyTrackerUi.Dto;

namespace StudyTrackerUi.Services;

public sealed class CacheService(IMemoryCache memoryCache)
{
    public static readonly string TasksKey = "StudyTracker/Tasks/";

    public void SetTasks(IEnumerable<TaskNode> tasks)
    {
        memoryCache.Set(TasksKey, tasks, TimeSpan.FromDays(1));
    }

    public IEnumerable<TaskNode> GetTasks()
    {
        var result = memoryCache.Get<IEnumerable<TaskNode>>(TasksKey);

        return result ?? [];
    }

    public void ClearTasks()
    {
        memoryCache.Remove(TasksKey);
    }
}