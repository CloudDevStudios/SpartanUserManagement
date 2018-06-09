using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SpartanEnvironment;
using SpartanExtensions.Objects;
using SpartanExtensions.Strings;
using SpartanLogging;
using SpartanSettings;

namespace SpartanUserManagement
{
    public class UserManagementApi : IUserManagement
    {
        private static ISetting _setting;
        private static ILogging _logging;
        private static IEnviroment _env;
        private static List<UserResponse> _users;
        private static string _configPath;
        public string Environment { get; set; }
        public string ConnectionString { get; set; }
        public string DevConnectionString { get; set; }
        public string StagingConnectionString { get; set; }
        public int Timeout { get; set; } = 20;

        public string EncryptKey { get; set; }
        public int PasswordMin { get; set; } = 6;
        public int PasswordMax { get; set; } = 12;
        public bool RequireUpperCase { get; set; } = true;
        public bool RequireLowerCase { get; set; } = true;
        public bool AvoidNoTwoSimilarChars { get; set; } = false;
        public bool RequireSpecialChars { get; set; } = true;
        public string SpecialChars { get; set; } = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";

        public UserManagementApi()
        {
            _setting = new Setting();
            _logging = new Logging();
            _env = new SpartanEnvironment.Environment();
            _configPath = _setting.CreateModuleSetting("usermanagement");
            SetDbParameters();

        }

        /// <summary>
        /// Ensures to set db parameters from the usermanagement.json 
        /// config file
        /// </summary>
        private void SetDbParameters()
        {
            var _errorMsg = string.Empty;
            if (!string.IsNullOrEmpty(_configPath))
            {
                try
                {

                    var cTemp = _configPath.LoadAsJsonType();
                    Environment = _env.GetUserVariable("Environment");
                    ConnectionString = cTemp.ContainsKey("ConnectionString") ? (string)cTemp.GetValue("ConnectionString") : "";
                    DevConnectionString = cTemp.ContainsKey("DevConnectionString") ? (string)cTemp.GetValue("DevConnectionString") : "";
                    StagingConnectionString = cTemp.ContainsKey("StagingConnectionString") ? (string)cTemp.GetValue("StagingConnectionString") : "";
                    if (!string.IsNullOrWhiteSpace(Environment))
                    {
                        //override due to env
                        switch (Environment)
                        {
                            case "Development":
                                ConnectionString = DevConnectionString;
                                break;
                            case "Staging":
                                ConnectionString = StagingConnectionString;
                                break;
                        }
                    }

                    //ConnectionString (default)
                    if (string.IsNullOrWhiteSpace(ConnectionString))
                    {
                        _errorMsg = $"A user managemenet configuration file was created in {_configPath}. This configuration is missing a connection string parameter";
                        _configPath.SaveTo(this.SerializeToJson());
                        _logging.Error("UserManagementApi:GetDbConnectinConnectionString", _errorMsg);
                    }

                    //timeout
                    Timeout = cTemp.ContainsKey("Timeout") ? (int)cTemp.GetValue("Timeout") : 20;

                    //EncryptKey
                    EncryptKey = cTemp.ContainsKey("EncryptKey") ? (string)cTemp.GetValue("EncryptKey") : "";
                    if (string.IsNullOrWhiteSpace(EncryptKey))
                    {
                        _errorMsg = $"A user managemenet configuration file was created in {_configPath}. This configuration is missing a Encrypt Key parameter";
                        _configPath.SaveTo(this.SerializeToJson());
                        _logging.Error("UserManagementApi:GetDbEncryptKey", _errorMsg);
                    }

                }
                catch (Exception ex)
                {
                    _configPath.SaveTo(this.SerializeToJson());
                    _logging.Error("UserManagementApi:GetDbConnectinConnectionString", ex.ToJsonString());
                }
            }
        }

        /// <summary>
        /// Returns list of All UserResponses
        /// </summary>
        /// <returns>Collection List of UserResponses</returns>
        public async Task<List<UserResponse>> GetActiveUsers()
        {
            _users = new List<UserResponse>();
            var _errorTitle = "UserManagementApi:GetAllUsers";

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                _logging.Error(_errorTitle, "No Connection to db was found");
                return _users;
            }

            return await Task.Run(() =>
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        _users = db.Query<UserResponse>(SqlQueries.GetActiveUsers_Sql, null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTitle, ex.ToString());
                }

                return _users;
            });
        }

        public async Task<UserResponse> GetUserById(Guid id)
        {
            var _userResponse = new UserResponse();
            var _user = new User();
            var _errorTitle = "UserManagementApi:GetUserById";

            //Id parameter check
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTitle, "Id can not be empty");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");

            return await Task.Run(() =>
            {
                _user = GetUser(SqlQueries.GetUserById_Sql, new { Id = id });
                if (_user != null)
                {
                    _userResponse = ResponseOk_ModelView(_user);
                }
                else
                {
                    _userResponse = ResponseError_ModelView(_errorTitle, "User was not found");
                }
                return _userResponse;
            });
        }

        /// <summary>
        /// Returns UserResponse by UserName
        /// </summary>
        /// <param name="username"></param>
        /// <returns>UserResponse (ModelView)</returns>
        public async Task<UserResponse> GetUserByUserName(string username)
        {
            var _userResponse = new UserResponse();
            var _user = new User();
            var _errorTitle = "UserManagementApi:GetUserByUserName";

            //UserName parameter check
            if (string.IsNullOrWhiteSpace(username))
                return ResponseError_ModelView(_errorTitle, "UserName can not be empty");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");

            return await Task.Run(() =>
            {
                _user = GetUser(SqlQueries.GetUserByUserName_Sql, new { UserName = username });
                if (_user != null)
                {
                    _userResponse = ResponseOk_ModelView(_user);
                }
                else
                {
                    _userResponse = ResponseError_ModelView(_errorTitle, "User was not found");
                }
                return _userResponse;
            });
        }

        /// <summary>
        /// retrieves UserResponse by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>UserResponse (ModelView)</returns>
        public async Task<UserResponse> GetUserByEmail(string email)
        {
            var _userResponse = new UserResponse();
            var _user = new User();
            var _errorTitle = "UserManagementApi:GetUserByEmail";

            //email parameter check
            if (string.IsNullOrWhiteSpace(email))
                return ResponseError_ModelView(_errorTitle, "email can not be empty");

            if (!email.IsValidEmail())
                return ResponseError_ModelView(_errorTitle, "email is not valid");


            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");

            return await Task.Run(() =>
            {
                _user = GetUser(SqlQueries.GetUserByEmail_Sql, new { Email = email });
                if (_user != null)
                {
                    _userResponse = ResponseOk_ModelView(_user);
                }
                else
                {
                    _userResponse = ResponseError_ModelView(_errorTitle, "User was not found");
                }
                return _userResponse;
            });
        }

        /// <summary>
        /// Add new users. 
        /// UserName and Email is a requirement.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> AddOrUpdateUserWithUserName(User user)
        {

            var _userResponse = new UserResponse();
            var _userTemp = new UserResponse();
            var _errorTitle = "UserManagementApi:AddUserByUserName";

            //User parameter
            if (user == null)
                return ResponseError_ModelView(_errorTitle, "user parameter can not be empty/null");

            //UserName parameter check
            if (string.IsNullOrWhiteSpace(user.UserName))
                return ResponseError_ModelView(_errorTitle, "UserName can not be empty");

            //UserName only one UserName can be added
            _userTemp = await GetUserByUserName(user.UserName);
            if (_userTemp.Status.Equals("ok") && !string.IsNullOrWhiteSpace(_userTemp.UserName))
                return ResponseError_ModelView(_errorTitle, "UserName already exist");

            //Email parameter check
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.IsValidEmail())
                return ResponseError_ModelView(_errorTitle, "Invalid Email");


            //Password parameter check
            var passwValidationPhrase = GetPasswordValidationPhrase(user.PasswordHash);
            if (passwValidationPhrase != "VALID")
                return ResponseError_ModelView(_errorTitle, passwValidationPhrase);

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");

            return await Task.Run(() =>
            {
                user.Id = (user.Id == Guid.Empty) ? Guid.NewGuid() : user.Id;
                user.PasswordHash = user.PasswordHash.EncryptString(EncryptKey);
                user.IsActive = true;
                user.AccessFailedCount = 0;
                user.LockEnabled = false;
                user.LockoutDescription = string.Empty;
                user.ReportsToId = null;
                user.DateCreated = DateTime.Now;
                int _rowsAffected = UserInsertOrUpdate(user);

                //Make sure execute fuction suceeded
                if (_rowsAffected <= 0)
                    return ResponseError_ModelView(_errorTitle, $"No records were inserted. Row Affected were {_rowsAffected}");

                //Only show ModelView response
                _userResponse = ResponseOk_ModelView(user);
                return _userResponse;
            });
        }

        /// <summary>
        /// Adds User. Email is a requirement (no username)
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> AddOrUpdateUserWithEmail(User user)
        {

            var _userResponse = new UserResponse();
            var _userTemp = new UserResponse();

            var _errorTitle = "UserManagementApi:AddUserByUserName";

            //User parameter
            if (user == null)
                return ResponseError_ModelView(_errorTitle, "user parameter can not be empty/null");

            //Email parameter check
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.IsValidEmail())
                return ResponseError_ModelView(_errorTitle, "Invalid Email");

            _userTemp = await GetUserByEmail(user.Email);
            if (_userTemp.Status.Equals("ok") && !string.IsNullOrWhiteSpace(_userTemp.Email))
                return ResponseError_ModelView(_errorTitle, "Email already exist");


            //Password parameter check
            var passwValidationPhrase = GetPasswordValidationPhrase(user.PasswordHash);
            if (passwValidationPhrase != "VALID")
                return ResponseError_ModelView(_errorTitle, passwValidationPhrase);

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");

            return await Task.Run(() =>
            {
                user.Id = (user.Id == Guid.Empty) ? Guid.NewGuid() : user.Id;
                user.PasswordHash = user.PasswordHash.EncryptString(EncryptKey);
                user.IsActive = true;
                user.AccessFailedCount = 0;
                user.LockEnabled = false;
                user.LockoutDescription = string.Empty;
                user.ReportsToId = null;
                user.DateCreated = DateTime.Now;
                int _rowsAffected = UserInsertOrUpdate(user);

                //Make sure execute fuction suceeded
                if (_rowsAffected <= 0)
                    return ResponseError_ModelView(_errorTitle, $"No records were inserted. Row Affected were {_rowsAffected}");

                //Only show ModelView response
                _userResponse = ResponseOk_ModelView(user);
                return _userResponse;
            });
        }

        private int UserInsertOrUpdate(User user)
        {
            int _rowsAffected = 0;
            var _errorTitle = "UserManagementApi:AddUser";
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    _rowsAffected = db.Execute(SqlQueries.UserAddOrUpdate_Sql, new
                    {
                        user.Id,
                        user.AppName,
                        user.UserName,
                        user.PasswordHash,
                        user.Type,
                        user.Company,
                        user.GivenName,
                        user.MiddleName,
                        user.SurName,
                        user.FullName,
                        user.NickName,
                        user.Gender,
                        user.MaritalStatus,
                        user.Email,
                        user.EmailSignature,
                        user.EmailProvider,
                        user.JobTitle,
                        user.BusinessPhone,
                        user.HomePhone,
                        user.MobilePhone,
                        user.FaxNumber,
                        user.Address,
                        user.Address1,
                        user.City,
                        user.State,
                        user.Province,
                        user.ZipCode,
                        user.Country,
                        user.CountryOrigin,
                        user.Citizenship,
                        user.WebPage,
                        user.Avatar,
                        user.About,
                        user.DoB,
                        user.IsActive,
                        user.AccessFailedCount,
                        user.LockEnabled,
                        user.LockoutDescription,
                        user.AccountNotes,
                        user.ReportsToId,
                        user.DateCreated,
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logging.Error(_errorTitle, ex.ToString());
            }

            return _rowsAffected;
        }

        /// <summary>
        /// Deletes all users. This function only works in "Development"
        /// environment.
        /// </summary>
        public async Task DeleteAllUsers()
        {
            int _rowsAffected = 0;
            bool _isValid = true;
            var _errorTitle = "UserManagementApi:DeleteAllUsers";
            var env = _env.GetUserVariable("Environment");

            //is Development?
            if (!env.Equals("Development"))
            {
                ResponseError_ModelView(_errorTitle, "No Connection to db was found");
                _isValid = false;
            }

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ResponseError_ModelView(_errorTitle, "No Connection to db was found");
                _isValid = false;
            }

            await Task.Run(() =>
            {
                try
                {
                    if (_isValid)
                    {
                        using (IDbConnection db = new SqlConnection(ConnectionString))
                        {
                            if (db.State == ConnectionState.Closed)
                                db.Open();

                            _rowsAffected = db.Execute(SqlQueries.DeleteAllUsers_Sql);
                            _logging.Info(_errorTitle, $"Users Rows Deleted {_rowsAffected}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTitle, ex.ToString());
                }
            });
        }

        /// <summary>
        /// Sets ActiveState to false
        /// </summary>
        /// <param name="id">userid</param>
        /// <param name="description">details why to delete</param>
        /// <returns></returns>
        public async Task<UserResponse> DeleteUserAccount(Guid id, string description)
        {
            return await SetActiveState(id, description, false);
        }

        /// <summary>
        /// Sets IsActive field.
        /// helper function to facilites (delete) disable/enable behaviors
        /// </summary>
        /// <param name="id">UserId (System.Guid)</param>
        /// <param name="description">description why the account is being disabled/enabled</param>
        /// <param name="isDisabled">true/false state</param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> SetActiveState(Guid id, string description, bool isActive)
        {
            var _userResponse = new UserResponse();
            var _user = new User();
            var _errorTitle = "UserManagementApi:DisableUserAccount";
            var _rowsAffected = 0;

            //Verify description parameter
            if (string.IsNullOrWhiteSpace(description))
                return ResponseError_ModelView(_errorTitle, "No description was found");

            //Verify id parameter
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTitle, "User Id is was not found");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No Connection to db was found");


            return await Task.Run(() =>
            {
                _user = GetUser(SqlQueries.GetUserById_Sql, new { Id = id });
                if (_user != null && !string.IsNullOrWhiteSpace(_user.Email))
                {
                    _user.IsActive = isActive;
                    _user.AccountNotes = description;
                    _user.DateCreated = DateTime.Now;
                    _rowsAffected = UserInsertOrUpdate(_user);

                    if (_rowsAffected <= 0)
                        return ResponseError_ModelView(_errorTitle, $"No records were inserted. Row Affected were {_rowsAffected} User Id: {_user.Id.ToString()}");
                }
                _userResponse = ResponseOk_ModelView(_user);
                return _userResponse;
            });
        }

        /// <summary>
        /// Sets lock state to true or false
        /// </summary>
        /// <param name="id">userid</param>
        /// <param name="description">details of setting the lock state</param>
        /// <param name="isLocked">true or false</param>
        /// <returns></returns>
        public async Task<UserResponse> SetLockState(Guid id, string description, bool isLocked)
        {
            var _userResponse = new UserResponse();
            var _desc = new System.Text.StringBuilder();
            var _user = new User();
            int _rowsAffected = 0;
            var _errorTitle = "UserManagementApi:LockUserAccount";

            //Verify description parameter
            if (string.IsNullOrWhiteSpace(description))
                return ResponseError_ModelView(_errorTitle, "No description was found");

            //Verify id parameter
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTitle, "User Id is was not found");

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTitle, "No db connection found");

            return await Task.Run(() =>
            {
                _user = GetUser(SqlQueries.GetUserById_Sql, new { Id = id });
                if (_user != null && !string.IsNullOrWhiteSpace(_user.Email))
                {
                    _user.LockEnabled = isLocked;
                    _user.LockoutDescription = description;
                    _user.DateCreated = DateTime.Now;
                    _rowsAffected = UserInsertOrUpdate(_user);

                    if (_rowsAffected <= 0)
                        return ResponseError_ModelView(_errorTitle, $"No records were inserted. Row Affected were {_rowsAffected} User Id: {_user.Id.ToString()}");
                }
                _userResponse = ResponseOk_ModelView(_user);
                return _userResponse;
            });
        }

        /// <summary>
        /// Resets Password Account
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="currentpassword">current active password</param>
        /// <param name="confirmedpassword">confirmed active account</param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> ResetPassword(string email, string currentpassword, string confirmedpassword)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:ResetPassword";
            var _rowsAffected = 0;

            //Verify email parameter
            if (string.IsNullOrWhiteSpace(email))
                return ResponseError_ModelView(_errorTitle, "No email was found");

            //Verify current password parameter
            if (string.IsNullOrWhiteSpace(currentpassword))
                return ResponseError_ModelView(_errorTitle, "No current password parameter found");

            //Verify confirmedpassword parameter
            if (string.IsNullOrWhiteSpace(confirmedpassword))
                return ResponseError_ModelView(_errorTitle, "No confirmed password  found");

            //Verify db password against the current password
            var _user = GetUser(SqlQueries.GetUserByEmail_Sql, new { Email = email });
            if (_user == null || string.IsNullOrWhiteSpace(email))
                return ResponseError_ModelView(_errorTitle, "User Account not Found");

            //Verify Encrypted Key parameter
            if (string.IsNullOrWhiteSpace(EncryptKey))
                return ResponseError_ModelView(_errorTitle, "No Encrypted Key found. see documentation for additional details");

            //Verify current passwords match
            if (!_user.PasswordHash.DecryptString(EncryptKey).Equals(currentpassword))
                return ResponseError_ModelView(_errorTitle, "current password does not match our system");

            return await Task.Run(() =>
            {
                _user.PasswordHash = confirmedpassword.EncryptString(EncryptKey);
                _user.AccountNotes = "password reset";
                _user.AccessFailedCount = 0;
                _user.LockEnabled = false;
                _user.LockoutDescription = string.Empty;
                _user.IsActive = true;
                _user.DateCreated = DateTime.Now;
                _rowsAffected = UserInsertOrUpdate(_user);

                if (_rowsAffected <= 0)
                    return ResponseError_ModelView(_errorTitle, $"reset password failed. Row Affected were {_rowsAffected} User Id: {_user.Id.ToString()}");

                _userResponse = ResponseOk_ModelView(_user);
                return _userResponse;
            });
        }

        /// <summary>
        /// Resets Password Account
        /// </summary>
        /// <param name="resetpassword">ResetPassword model</param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> ResetPassword(ResetPassword resetpassword)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:ResetPassword";
            if (resetpassword != null && (!string.IsNullOrWhiteSpace(resetpassword.Email)
                && !string.IsNullOrWhiteSpace(resetpassword.ConfirmedPassword)
                    && !string.IsNullOrWhiteSpace(resetpassword.Password)))
            {
                _userResponse = await ResetPassword(resetpassword.Email, resetpassword.Password, resetpassword.ConfirmedPassword);
            }
            else
            {
                _userResponse = ResponseError_ModelView(_errorTitle, "No Encrypted Key found. see documentation for additional details");
            }

            return _userResponse;
        }

        /// <summary>
        /// Common helper to retrieve User from Database
        /// </summary>
        /// <param name="spname">Store Procedure Name</param>
        /// <param name="param">Parameters Object</param>
        /// <returns></returns>
        private User GetUser(string spname, object param)
        {
            var _user = new User();
            var _errorTitle = "UserManagementApi:GetUser";
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    _user = db.QueryFirst<User>(spname, param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logging.Error(_errorTitle, ex.ToString());
                _user = null;
            }

            return _user;
        }

        /// <summary>
        /// Common helper to validate password and settings 
        /// </summary>
        /// <param name="value">password to test</param>
        /// <returns>errors or VALID string phrase</returns>
        private string GetPasswordValidationPhrase(string value)
        {
            if (value.Length < PasswordMin || value.Length > PasswordMax)
                return $"min {PasswordMin} chars, max {PasswordMax} chars";

            if (value.Contains(" "))
                return "Can not have white space";

            if (RequireUpperCase)
            {
                if (!value.Any(char.IsUpper))
                    return "At least 1 upper case letter";
            }

            if (RequireLowerCase)
            {
                if (!value.Any(char.IsLower))
                    return "At least 1 lower case letter";
            }

            if (AvoidNoTwoSimilarChars)
            {
                for (int i = 0; i < value.Length - 1; i++)
                {
                    if (value[i] == value[i + 1])
                        return "No two similar chars consecutively";
                }
            }

            //At least 1 special char
            if (RequireSpecialChars)
            {
                char[] specialCharactersArray = SpecialChars.ToCharArray();
                foreach (char c in specialCharactersArray)
                {
                    if (value.Contains(c))
                    {
                        return "VALID";
                    }
                }
                return "At least 1 special char";
            }

            return "unknown state";
        }

        /// <summary>
        /// Common helper to response User - Errors
        /// </summary>
        /// <param name="title">name of error</param>
        /// <param name="msg">description</param>
        /// <returns>UserResponse</returns>
        private UserResponse ResponseError_ModelView(string title, string msg)
        {
            var _userResponse = new UserResponse();
            _logging.Error(title, msg);
            _userResponse.Status = "error";
            _userResponse.Msg = msg;
            return _userResponse;
        }

        /// <summary>
        /// Common helper to Response User - Ok
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>UserResponse</returns>
        private UserResponse ResponseOk_ModelView(User user)
        {
            UserResponse userResponse = new UserResponse
            {
                Status = "ok",
                Msg = "",
                Id = user.Id,
                AppName = user.AppName,
                UserName = user.UserName,
                Type = user.Type,
                Company = user.Company,
                GivenName = user.GivenName,
                MiddleName = user.MiddleName,
                SurName = user.SurName,
                NickName = user.NickName,
                FullName = user.FullName,
                Gender = user.Gender,
                MaritalStatus = user.MaritalStatus,
                Email = user.Email,
                EmailSignature = user.EmailSignature,
                EmailProvider = user.EmailProvider,
                JobTitle = user.JobTitle,
                BusinessPhone = user.BusinessPhone,
                HomePhone = user.HomePhone,
                MobilePhone = user.MobilePhone,
                FaxNumber = user.FaxNumber,
                Address = user.Address,
                Address1 = user.Address1,
                City = user.City,
                State = user.State,
                Province = user.Province,
                ZipCode = user.ZipCode,
                Country = user.Country,
                CountryOrigin = user.CountryOrigin,
                Citizenship = user.Citizenship,
                WebPage = user.WebPage,
                Avatar = user.Avatar,
                About = user.About,
                DoB = user.DoB,
                IsActive = user.IsActive,
                AccessFailedCount = user.AccessFailedCount,
                LockEnabled = user.LockEnabled,
                LockoutDescription = user.LockoutDescription,
                ReportsToId = user.ReportsToId,
                DateCreated = user.DateCreated
            };
            return userResponse;
        }

        /// <summary>
        /// Validate Account via Email
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        public async Task<UserResponse> LoginByEmail(string email, string password)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:LoginByEmail";

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return ResponseError_ModelView(_errorTitle, "No email/passoword params found");

            //check email validation
            if (!email.IsValidEmail())
                return ResponseError_ModelView(_errorTitle, "Invalid Email");


            return await Task.Run(() =>
            {
                var _user = GetUser(SqlQueries.GetUserByEmail_Sql, new { Email = email });
                if (_user != null)
                {
                    if (!_user.PasswordHash.DecryptString(EncryptKey).Equals(password))
                        return ResponseError_ModelView(_errorTitle, "Invalid Password");

                    _userResponse = ResponseOk_ModelView(_user);
                }
                else
                {
                    _userResponse = ResponseError_ModelView(_errorTitle, "User was not found");
                }

                return _userResponse;
            });

        }

        /// <summary>
        /// Validate Account via Email
        /// </summary>
        /// <param name="loginemail">LoginEmail model</param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> LoginByEmail(LoginEmail loginemail)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:LoginByEmail";

            if (loginemail != null && (!string.IsNullOrWhiteSpace(loginemail.Email)
                    && !string.IsNullOrWhiteSpace(loginemail.Password)))
            {
                _userResponse = await LoginByEmail(loginemail.Email, loginemail.Password);
            }
            else
            {
                _userResponse = ResponseError_ModelView(_errorTitle, "No email/passoword params found");
            }

            return _userResponse;
        }


        public async Task<UserResponse> LoginByUserName(string username, string password)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:LoginByUserName";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return ResponseError_ModelView(_errorTitle, "No username/passoword params found");

            return await Task.Run(() =>
            {
                var _user = GetUser(SqlQueries.GetUserByUserName_Sql, new { UserName = username });
                if (_user != null)
                {
                    if (!_user.PasswordHash.DecryptString(EncryptKey).Equals(password))
                        return ResponseError_ModelView(_errorTitle, "Invalid Password");

                    //check for active or lock state
                    _userResponse = ResponseOk_ModelView(_user);
                }
                else
                {
                    _userResponse = ResponseError_ModelView(_errorTitle, "User was not found");
                }

                return _userResponse;
            });
        }

        public async Task<UserResponse> LoginByUserName(LoginUser loginuser)
        {
            var _userResponse = new UserResponse();
            var _errorTitle = "UserManagementApi:LoginByUserName";

            if (loginuser != null && (!string.IsNullOrWhiteSpace(loginuser.UserName)
                    && !string.IsNullOrWhiteSpace(loginuser.Password)))
            {
                _userResponse = await LoginByUserName(loginuser.UserName, loginuser.Password);
            }
            else
            {
                _userResponse = ResponseError_ModelView(_errorTitle, "No username/passoword params found");
            }

            return _userResponse;
        }


        public async Task<Role> AddRole(string name, bool isActive = true)
        {
            var _role = new Role();
            var _errorTitle = "UserManagementApi:AddRole";
            var _rowsAffected = 0;
            //check for name
            if (string.IsNullOrWhiteSpace(name))
            {
                _logging.Error(_errorTitle, "name params is empty");
                return null;
            }

            //TODO::: check if the role exist

            return await Task.Run(() =>
            {
                try
                {
                    var id = Guid.NewGuid();
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        _role.Id = id;
                        _role.RoleName = name;
                        _role.IsActive = isActive;
                        _rowsAffected = db.Execute(SqlQueries.AddRole_Sql, _role);
                        if (_rowsAffected <= 0)
                        {
                            _logging.Error(_errorTitle, "no rows were affected during role insertation");
                            return null;
                        }

                    }

                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTitle, ex.ToString());
                    _role = null;
                }
                return _role;
            });
        }
    }
}