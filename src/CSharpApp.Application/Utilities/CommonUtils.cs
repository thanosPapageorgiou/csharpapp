
namespace CSharpApp.Application.Utilities
{
    public static class CommonUtils
    {
        public static bool IsRunningFromUnitTest()
        {
            var isUnitTest = Environment.GetEnvironmentVariable("IS_UNIT_TEST");
            return isUnitTest == "true";
        }
    }
}
