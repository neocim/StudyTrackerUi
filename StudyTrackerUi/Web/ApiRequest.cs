using Microsoft.Extensions.Configuration;

namespace StudyTrackerUi.Web;

public sealed class ApiRequest(IConfiguration configuration)
{
    public readonly string BaseUrl = configuration["ApiUrl"]!;

    public UserEndpoints User(Guid id)
    {
        return new UserEndpoints(BaseUrl, id);
    }

    public sealed record UserEndpoints(string BaseUri, Guid UserId)
    {
        public TaskEndpoints Task => new($"{BaseUri}/users/{UserId}");
    }

    public sealed record TaskEndpoints(string EndpointBase)
    {
        public string Create => $"{EndpointBase}/tasks";
        public string Update => $"{EndpointBase}/tasks";
        public string Delete => $"{EndpointBase}/tasks";
        public string GetMany => $"{EndpointBase}/tasks";

        public string GetOne(Guid taskId)
        {
            return $"{EndpointBase}/tasks/{taskId}";
        }

        public string CreateSubTask(Guid parentTaskId)
        {
            return $"{EndpointBase}/tasks/{parentTaskId}/subtasks";
        }
    }
}