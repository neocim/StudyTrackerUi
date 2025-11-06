using Microsoft.Extensions.Configuration;

namespace StudyTrackerUi.Api.Queries;

public sealed class ApiQuery(IConfiguration Configuration)
{
    public readonly string ApiUri = Configuration["ApiUri"]!;

    public UserEndpoints User(Guid id)
    {
        return new UserEndpoints(ApiUri, id);
    }

    public sealed record UserEndpoints(string ApiUri, Guid UserId)
    {
        public TaskEndpoints Task()
        {
            return new TaskEndpoints(ApiUri, UserId);
        }
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