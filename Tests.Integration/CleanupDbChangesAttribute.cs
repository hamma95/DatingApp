using System.Data.Common;
using System.Reflection;
using Xunit.Sdk;

namespace Tests.Integration;

public class CleanupDbChangesAttribute : BeforeAfterTestAttribute
{
    private readonly TestDatabaseManager _testDatabaseManager = new();

    public override void After(MethodInfo methodUnderTest)
    {
        _testDatabaseManager.CleanDatabase();
    }
}