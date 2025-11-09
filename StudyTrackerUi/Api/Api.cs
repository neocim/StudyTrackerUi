using System.Net;
using ErrorOr;
using Newtonsoft.Json;
using TaskDto = StudyTrackerUi.Api.Dto.Task;

namespace StudyTrackerUi.Api;

public sealed class Api(HttpClient httpClient, ApiQuery apiQuery)
{
    public async Task<ErrorOr<TaskDto>> GetTask(Guid userId, Guid taskId)
    {
        var request = apiQuery.User(userId).Task.GetOne(taskId);
        var response = await httpClient.GetAsync(request);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var task = JsonConvert.DeserializeObject<TaskDto>(jsonResponse)!;

        return task;
    }
}