namespace StudyTrackerUi.Api;

public sealed class Api(HttpClient httpClient, ApiQuery apiQuery)
{
    public async Task GetTask(Guid userId, Guid taskId)
    {
        var request = apiQuery.User(userId).Task.GetOne(taskId);
        var response = await httpClient.GetAsync(request);
        var jsonResponse = await response.Content.ReadAsStringAsync();
    }
}