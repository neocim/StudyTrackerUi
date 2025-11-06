using Microsoft.Extensions.Configuration;

namespace StudyTrackerUi.Api;

public sealed class ApiQuery(IConfiguration configuration)
{
    public readonly string ApiUri = configuration["ApiUri"]!;

    public UserEndpoints User(Guid id)
    {
        return new UserEndpoints(ApiUri, id);
    }

    public sealed record UserEndpoints(string ApiUri, Guid UserId)
    {
        public TaskEndpoints Task => new(ApiUri, UserId);
    }

    public sealed record TaskEndpoints(string ApiUri, Guid UserId)
    {
        public string Create => $"{ApiUri}/user/{UserId}/tasks";
        public string Update => $"{ApiUri}/user/{UserId}/tasks";
        public string Delete => $"{ApiUri}/user/{UserId}/tasks";
        public string GetMany => $"{ApiUri}/user/{UserId}/tasks";

        public string GetOne(Guid taskId)
        {
            return $"{ApiUri}/user/{UserId}/tasks/{taskId}";
        }

        public string CreateSubTask(Guid parentTaskId)
        {
            return $"{ApiUri}/user/{UserId}/tasks/{parentTaskId}/subtasks";
        }
    }
}