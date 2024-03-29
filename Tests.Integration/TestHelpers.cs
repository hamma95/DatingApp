namespace Tests.Integration;

public static class TestHelpers
{
    public static string GetAppSettingsForTestsFilePath()
    {
        var pathToFile = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
        return Path.Combine(pathToFile.FullName, "appsettings.IntegrationTests.json");
    }
}