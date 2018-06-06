using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpartanEnvironment;

namespace SpartanUserManagement.Test
{
    [TestClass]
    public class UserManagementApiTest
    {
        private UserManagementApi _users;
        private User _user;
        private static IEnviroment _env;
        [TestMethod]
        public void TestUserManagementSettings()
        {
            _env = new Environment();
            _env.SetUserVariable("Environment", "Development");
            var _ums = new UserManagementApi();
            var _testConnection = _ums.ConnectionString;
            var _testEnvString = _ums.Environment;
            Assert.IsTrue(_testEnvString.Equals("Development"));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_testConnection), "No Connection found");

        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestAddNewUserByUserNameAsync()
        {
            _env = new Environment();
            _users = new UserManagementApi();
            _env.SetUserVariable("Environment", "Development");
            _user = new User();

            //1- Create Dummy User
            _user.Id = System.Guid.Parse("F037567D-54BC-4044-A6F4-66A7E85A0E34");
            _user.UserName = "cperez";
            _user.GivenName = "carlos";
            _user.SurName = "perez";
            _user.PasswordHash = "TestGoog!e1";
            _user.Email = "cperez@donotreply.com";

            //2- Delete records only for Development environment
            await _users.DeleteAllUsers();


            //3- Add User Records
            var _userResponse = await _users.AddOrUpdateUserWithUserName(_user);
            Assert.IsTrue(_userResponse.Status.Equals("ok"), _userResponse.Msg);

            //4- Update User - changed username and same password
            _user.UserName = "cperez1";
            _user.PasswordHash = "TestGoog!e1";
            _userResponse = await _users.AddOrUpdateUserWithUserName(_user);
            Assert.IsTrue(_userResponse.Status.Equals("ok"), _userResponse.Msg);

            //5- Lock The Account
            _userResponse = await _users.SetLockState(_user.Id, "This account is locked due to payments", true);
            Assert.IsTrue(_userResponse.LockEnabled, _userResponse.Msg);

            System.Threading.Thread.Sleep(500);

            //5- Unlock The Account
            _userResponse = await _users.SetLockState(_user.Id, "Payments Received for $200", false);
            Assert.IsTrue(!_userResponse.LockEnabled, _userResponse.Msg);

            //6- Disable the account and verify
            System.Threading.Thread.Sleep(500);
            _userResponse = await _users.SetActiveState(_user.Id, "Deleting the Account for Temp reasons!", false);
            Assert.IsTrue(!_userResponse.IsActive, "failed to disable the user account");


            //7- Enable the account and verify
            System.Threading.Thread.Sleep(500);
            _userResponse = await _users.SetActiveState(_user.Id, "Found the Problem. Account was enable after receiving the email from Peter", true);
            Assert.IsTrue(_userResponse.IsActive, "failed to enable the user account");

            //8- Find User By UserName
            _userResponse = await _users.GetUserByUserName("cperez1");
            Assert.IsTrue((_userResponse.Status.Equals("ok") && _userResponse.Email.Equals("cperez@donotreply.com")), _userResponse.Msg);

            //9- Find User By Email
            _userResponse = await _users.GetUserByEmail("cperez@donotreply.com");
            Assert.IsTrue((_userResponse.Status.Equals("ok") && _userResponse.UserName.Equals("cperez1")), _userResponse.Msg);

            //10- Delete the User Account 
            _userResponse = await _users.DeleteUserAccount(_user.Id, "Testing Delete Account");
            var _userList = await _users.GetActiveUsers();
            Assert.IsTrue(_userList.Count == 0, "Failed to Delete Account");

            //7- Verify User Counts
            _userList = await _users.GetActiveUsers();
            Assert.IsTrue(_userList.Count > 0, "Failed to Retrived All Users");
        }
    }
}
