namespace StudyTrackerUi.Api.Security;

public class SessionService
{
    public static string StorageKey = "AuthToken";
    public static SessionService Instance = new();
    public BearerTokenInfo BearerTokenInfo { get; private set; } = null!;
}