using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpartanEnvironment;
//using System;

namespace SpartanUserManagement.Test
{
    [TestClass]
    public class UserManagementApiTest
    {
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
            var _users = new UserManagementApi();
            _env.SetUserVariable("Environment", "Development");

            var user = new SpartanUserManagement.User();
            var user2 = new SpartanUserManagement.User();
            var user3 = new SpartanUserManagement.User();
            user.UserName = "cperez";
            user.FirstName = "carlos";
            user.LastName = "perez";
            user.PasswordHash = "TestGoog!e1";
            user.Email = "cperez@donotreply.com";
            //user.DateCreated = DateTime.Now;

            //With User
            user2.UserName = "hPerez";
            user2.FirstName = "Hermes";
            user2.LastName = "Perez";
            user2.PasswordHash = "PerezGoolG!1";
            user2.Email = "hperez@donotreply.com";

            //No user but Email

            user3.FirstName = "Jorge";
            user3.LastName = "Perez";
            user3.PasswordHash = "PerezGool!";
            user3.Email = "jperez@donotreply.com";

            //Delete records only for Development environment
            await _users.DeleteAllUsers();
            //Add user1
            var _userResponse = await _users.AddUserWithUserName(user);
            Assert.IsTrue(_userResponse.Status.Equals("ok"), _userResponse.Msg);

            //Add User by UserName
            _userResponse = await _users.AddUserWithUserName(user2);
            Assert.IsTrue(_userResponse.Status.Equals("ok"), _userResponse.Msg);

            //Add User by Email
            _userResponse = await _users.AddUserWithEmail(user3);
            Assert.IsTrue(_userResponse.Status.Equals("ok"), _userResponse.Msg);

            //disable the account and verify
            System.Threading.Thread.Sleep(500);
            var tempId = _userResponse.Id;
            _userResponse = await _users.DisableUserAccount(tempId);
            Assert.IsTrue(!_userResponse.IsActive, "failed to disable the user account");


            var _userList = await _users.GetActiveUsers();
            Assert.IsTrue(_userList.Count > 0, "Failed to Retrived All Users");
        }
    }
}
