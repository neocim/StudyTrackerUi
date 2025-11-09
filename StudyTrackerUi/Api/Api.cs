using System.Net;
using System.Text;
using ErrorOr;
using Newtonsoft.Json;
using TaskDto = StudyTrackerUi.Api.Dto.Task;

namespace StudyTrackerUi.Api;

public sealed class Api(HttpClient httpClient, ApiRequest apiRequest)
{
    public async Task<ErrorOr<TaskDto>> GetTask(Guid userId, Guid taskId)
    {
        using var response = await httpClient.GetAsync(apiRequest.User(userId).Task.GetOne(taskId));

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var task = JsonConvert.DeserializeObject<TaskDto>(jsonResponse)!;

        return task;
    }

    public async Task<ErrorOr<Created>> CreateTask(Guid userId, Guid taskId, string name,
        string description, DateOnly beginDate, DateOnly endDate)
    {
        var newTask = new
        {
            name,
            description,
            begin_date = beginDate,
            end_date = endDate
        };
        using var jsonContent = new StringContent(
            JsonConvert.SerializeObject(newTask),
            Encoding.UTF8,
            "application/json");

        using var response =
            await httpClient.PostAsync(apiRequest.User(userId).Task.Create, jsonContent);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.Created)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        return Result.Created;
    }
}