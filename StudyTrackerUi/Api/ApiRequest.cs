using Microsoft.Extensions.Configuration;

namespace StudyTrackerUi.Api;

public sealed class ApiRequest(IConfiguration configuration)
{
    public readonly string BaseUrl = configuration["ApiUrl"]!;

    public UserEndpoints User(Guid id)
    {
        return new UserEndpoints(BaseUrl, id);
    }

    public sealed record UserEndpoints(string BaseUri, Guid UserId)
    {
        public TaskEndpoints Task => new($"{BaseUri}/users", UserId);
    }

    public sealed record TaskEndpoints(string EndpointBase, Guid UserId)
    {
        public string Create => $"{EndpointBase}/{UserId}/tasks";
        public string Update => $"{EndpointBase}/{UserId}/tasks";
        public string Delete => $"{EndpointBase}/{UserId}/tasks";
        public string GetMany => $"{EndpointBase}/{UserId}/tasks";

        public string GetOne(Guid taskId)
        {
            return $"{EndpointBase}/{UserId}/tasks/{taskId}";
        }

        public string CreateSubTask(Guid parentTaskId)
        {
            return $"{EndpointBase}/{UserId}/tasks/{parentTaskId}/subtasks";
        }
    }
}