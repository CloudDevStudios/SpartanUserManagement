using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpartanEnvironment;
namespace SpartanUserManagement.Test
{
    [TestClass]
    public class UserManagementApiTest
    {
        private static IEnviroment _env;
        [TestMethod]
        public void TestUserManagementSettings()
        {
            _env = new SpartanEnvironment.Environment();
            _env.SetUserVariable("Environment", "Development");
            var _ums = new UserManagementApi();
            var _testConnection = _ums.ConnectionString;
            var _testEnvString = _ums.Environment;

            Assert.IsTrue(_testEnvString.Equals("Development"));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_testConnection), "No Connection found");

        }
    }
}
