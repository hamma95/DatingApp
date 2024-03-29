namespace Tests.Integration;

[CleanupDbChanges]
public abstract class BaseTests : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient _client;
    
    public BaseTests(CustomWebApplicationFactory factory)
    {
        _client = factory.HttpClient;
    }
}