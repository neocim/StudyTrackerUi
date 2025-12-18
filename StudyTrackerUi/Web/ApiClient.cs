using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ErrorOr;
using Newtonsoft.Json;
using StudyTrackerUi.Dto;
using TaskDto = StudyTrackerUi.Dto.Task;

namespace StudyTrackerUi.Web;

public sealed class ApiClient
{
    private readonly ApiRequest _apiRequest;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient, ApiRequest apiRequest)
    {
        _httpClient = httpClient;
        _apiRequest = apiRequest;
    }

    public async Task<ErrorOr<TaskDto>> CreateTask(
        Guid userId,
        string name,
        string description,
        DateOnly beginDate,
        DateOnly endDate)
    {
        var newTask = new
        {
            name,
            description,
            beginDate,
            endDate
        };

        using var jsonContent = new StringContent(
            JsonConvert.SerializeObject(newTask),
            Encoding.UTF8,
            "application/json");

        using var response =
            await _httpClient.PostAsync(_apiRequest.User(userId).Task.Create, jsonContent);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.Created)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var task = JsonConvert.DeserializeObject<TaskDto>(jsonResponse)!;

        return task;
    }

    public async Task<ErrorOr<TaskDto>> CreateSubTask(
        Guid userId,
        Guid parentTaskId,
        string name,
        string description,
        DateOnly beginDate,
        DateOnly endDate)
    {
        var newSubTask = new
        {
            name,
            description,
            beginDate,
            endDate
        };

        using var jsonContent = new StringContent(
            JsonConvert.SerializeObject(newSubTask),
            Encoding.UTF8,
            "application/json");

        using var response =
            await _httpClient.PostAsync(_apiRequest.User(userId).Task.CreateSubTask(parentTaskId),
                jsonContent);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.Created)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var task = JsonConvert.DeserializeObject<TaskDto>(jsonResponse)!;

        return task;
    }

    public async Task<ErrorOr<TaskDto>> GetTask(Guid userId, Guid taskId)
    {
        using var response =
            await _httpClient.GetAsync(_apiRequest.User(userId).Task.GetOne(taskId));

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var task = JsonConvert.DeserializeObject<TaskDto>(jsonResponse)!;

        return task;
    }

    public async Task<ErrorOr<IEnumerable<TaskNode>>> GetTasks(Guid userId)
    {
        using var response = await _httpClient.GetAsync(_apiRequest.User(userId).Task.GetMany);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var tasks = JsonConvert.DeserializeObject<IEnumerable<TaskNode>>(jsonResponse)!;

        return tasks.ToErrorOr();
    }

    public async Task<ErrorOr<IEnumerable<TaskNode>>> GetSubTasks(Guid userId, Guid taskId)
    {
        using var response =
            await _httpClient.GetAsync(_apiRequest.User(userId).Task.GetMany);

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            return Error.Custom(999, response.StatusCode.ToString(), jsonResponse);

        var subTasks = JsonConvert.DeserializeObject<IEnumerable<TaskNode>>(jsonResponse)!;

        return subTasks.ToErrorOr();
    }

    public void SetAuthHeader(string bearerToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);
    }
}