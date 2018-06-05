﻿using System;
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
            var _errorTile = "UserManagementApi:GetAllUsers";

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                _logging.Error(_errorTile, "No Connection to db was found");
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

                        _users = db.Query<UserResponse>(SqlQueries.GetActiveUsers_Sql,null, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTile, ex.ToString());
                }

                return _users;
            });
        }

        public async Task<UserResponse> GetUserById(Guid id)
        {
            var _userResponse = new UserResponse();
            var _user = new User();
            var _errorTile = "UserManagementApi:GetUserById";

            //Id parameter check
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTile, "Id can not be empty");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");

            return await Task.Run(() =>
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        _user = db.QueryFirst<User>(SqlQueries.GetUserById_Sql, new { Id = id });
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error("UserManagementApi:GetUserById", ex.ToString());
                }
                _userResponse = ResponseOk_ModelView(_user);
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
            var _errorTile = "UserManagementApi:GetUserByUserName";

            //UserName parameter check
            if (string.IsNullOrWhiteSpace(username))
                return ResponseError_ModelView(_errorTile, "UserName can not be empty");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");

            return await Task.Run(() =>
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        _user = db.QueryFirst<User>(SqlQueries.GetUserByUserName_Sql, new { UserName = username }, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTile, ex.ToString());
                }

                _userResponse = ResponseOk_ModelView(_user);
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
            var _errorTile = "UserManagementApi:GetUserByEmail";

            //email parameter check
            if (string.IsNullOrWhiteSpace(email))
                return ResponseError_ModelView(_errorTile, "email can not be empty");

            if (!email.IsValidEmail())
                return ResponseError_ModelView(_errorTile, "email is not valid");


            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");

            return await Task.Run(() =>
            {
                try
                {
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        _user = db.QueryFirst<User>(SqlQueries.GetUserByEmail_Sql, new { Email = email }, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTile, ex.ToString());
                }

                _userResponse = ResponseOk_ModelView(_user);
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
            var _errorTile = "UserManagementApi:AddUserByUserName";

            //User parameter
            if (user == null)
                return ResponseError_ModelView(_errorTile, "user parameter can not be empty/null");

            //UserName parameter check
            if (string.IsNullOrWhiteSpace(user.UserName))
                return ResponseError_ModelView(_errorTile, "UserName can not be empty");

            //UserName only one UserName can be added
            _userTemp = await GetUserByUserName(user.UserName);
            if (_userTemp.Status.Equals("ok") && !string.IsNullOrWhiteSpace(_userTemp.UserName))
                return ResponseError_ModelView(_errorTile, "UserName already exist");

            //Email parameter check
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.IsValidEmail())
                return ResponseError_ModelView(_errorTile, "Invalid Email");


            //Verify if User exist by email
            if (user.Id != Guid.Empty)
            {
                _userTemp = await GetUserById(user.Id);
                if (_userTemp != null && !string.IsNullOrWhiteSpace(_userTemp.Email))
                    return ResponseError_ModelView(_errorTile, "User with this email already exist");
            }

            //Password parameter check
            var passwValidationPhrase = GetPasswordValidationPhrase(user.PasswordHash);
            if (passwValidationPhrase != "VALID")
                return ResponseError_ModelView(_errorTile, passwValidationPhrase);

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");

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
                    return ResponseError_ModelView(_errorTile, $"No records were inserted. Row Affected were {_rowsAffected}");

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
            
            var _errorTile = "UserManagementApi:AddUserByUserName";

            //User parameter
            if (user == null)
                return ResponseError_ModelView(_errorTile, "user parameter can not be empty/null");

            //Email parameter check
            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.IsValidEmail())
                return ResponseError_ModelView(_errorTile, "Invalid Email");

            _userTemp = await GetUserByEmail(user.Email);
            if (_userTemp.Status.Equals("ok") && !string.IsNullOrWhiteSpace(_userTemp.Email))
                return ResponseError_ModelView(_errorTile, "Email already exist");

            //Verify if User exist by email
            if (user.Id != Guid.Empty)
            {
                var userTemp = await GetUserById(user.Id);
                if (userTemp != null && !string.IsNullOrWhiteSpace(userTemp.Email))
                    return ResponseError_ModelView(_errorTile, "User with this email already exist");
            }

            //Password parameter check
            var passwValidationPhrase = GetPasswordValidationPhrase(user.PasswordHash);
            if (passwValidationPhrase != "VALID")
                return ResponseError_ModelView(_errorTile, passwValidationPhrase);

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");

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
                    return ResponseError_ModelView(_errorTile, $"No records were inserted. Row Affected were {_rowsAffected}");

                //Only show ModelView response
                _userResponse = ResponseOk_ModelView(user);
                return _userResponse;
            });
        }

        private int UserInsertOrUpdate(User user)
        {
            int _rowsAffected = 0;
            var _errorTile = "UserManagementApi:AddUser";
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
                _logging.Error(_errorTile, ex.ToString());
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
            var _errorTile = "UserManagementApi:DeleteAllUsers";
            var env = _env.GetUserVariable("Environment");

            //is Development?
            if (!env.Equals("Development"))
            {
                ResponseError_ModelView(_errorTile, "No Connection to db was found");
                _isValid = false;
            }

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ResponseError_ModelView(_errorTile, "No Connection to db was found");
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
                            _logging.Info(_errorTile, $"Users Rows Deleted {_rowsAffected}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logging.Error(_errorTile, ex.ToString());
                }
            });
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
            var _errorTile = "UserManagementApi:DisableUserAccount";
            var _rowsAffected = 0;

            //Verify description parameter
            if (string.IsNullOrWhiteSpace(description))
                return ResponseError_ModelView(_errorTile, "No description was found");

            //Verify id parameter
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTile, "User Id is was not found");

            //ConnectionString check
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No Connection to db was found");


            return await Task.Run(() =>
            {
                _user = GetUser(id);
                if (_user != null && !string.IsNullOrWhiteSpace(_user.Email))
                {
                    _user.IsActive = isActive;
                    _user.AccountNotes = description;
                    _user.DateCreated = DateTime.Now;
                    _rowsAffected = UserInsertOrUpdate(_user);

                    if (_rowsAffected <= 0)
                        return ResponseError_ModelView(_errorTile, $"No records were inserted. Row Affected were {_rowsAffected} User Id: {_user.Id.ToString()}");
                }
                _userResponse = ResponseOk_ModelView(_user);
                return _userResponse;
            });
        }

        public async Task<UserResponse> SetLockState(Guid id, string description, bool isLocked)
        {
            var _userResponse = new UserResponse();
            var _desc = new System.Text.StringBuilder();
            var _user = new User();
            int _rowsAffected = 0;
            var _errorTile = "UserManagementApi:LockUserAccount";

            //Verify description parameter
            if (string.IsNullOrWhiteSpace(description))
                return ResponseError_ModelView(_errorTile, "No description was found");

            //Verify id parameter
            if (id == Guid.Empty)
                return ResponseError_ModelView(_errorTile, "User Id is was not found");

            //Connection String
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return ResponseError_ModelView(_errorTile, "No db connection found");

            return await Task.Run(() =>
            {
                _user = GetUser(id);
                if (_user != null && !string.IsNullOrWhiteSpace(_user.Email))
                {
                    _user.LockEnabled = isLocked;
                    _user.LockoutDescription = description;
                    _user.DateCreated = DateTime.Now;
                    _rowsAffected = UserInsertOrUpdate(_user);

                    if (_rowsAffected <= 0)
                        return ResponseError_ModelView(_errorTile, $"No records were inserted. Row Affected were {_rowsAffected} User Id: {_user.Id.ToString()}");
                }
                _userResponse = ResponseOk_ModelView(_user);
                return _userResponse;
            });
        }


        private User GetUser(Guid id)
        {
            var _user = new User();
            var _errorTitle = "UserManagementApi:GetUser";
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    _user = db.QueryFirst<User>(SqlQueries.GetUserById_Sql, new { Id = id }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logging.Error(_errorTitle, ex.ToString());
                _user = null;
            }

            return _user;
        }
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
        private UserResponse ResponseError_ModelView(string title, string msg)
        {
            var _userResponse = new UserResponse();
            _logging.Error(title, msg);
            _userResponse.Status = "error";
            _userResponse.Msg = msg;
            return _userResponse;
        }
        private UserResponse ResponseOk_ModelView(User user)
        {
            UserResponse userResponse = new UserResponse();
            userResponse.Status = "ok";
            userResponse.Msg = "";
            userResponse.Id = user.Id;
            userResponse.AppName = user.AppName;
            userResponse.UserName = user.UserName;
            userResponse.Type = user.Type;
            userResponse.Company = user.Company;
            userResponse.GivenName = user.GivenName;
            userResponse.MiddleName = user.MiddleName;
            userResponse.SurName = user.SurName;
            userResponse.NickName = user.NickName;
            userResponse.FullName = user.FullName;
            userResponse.Gender = user.Gender;
            userResponse.MaritalStatus = user.MaritalStatus;
            userResponse.Email = user.Email;
            userResponse.EmailSignature = user.EmailSignature;
            userResponse.EmailProvider = user.EmailProvider;
            userResponse.JobTitle = user.JobTitle;
            userResponse.BusinessPhone = user.BusinessPhone;
            userResponse.HomePhone = user.HomePhone;
            userResponse.MobilePhone = user.MobilePhone;
            userResponse.FaxNumber = user.FaxNumber;
            userResponse.Address = user.Address;
            userResponse.Address1 = user.Address1;
            userResponse.City = user.City;
            userResponse.State = user.State;
            userResponse.Province = user.Province;
            userResponse.ZipCode = user.ZipCode;
            userResponse.Country = user.Country;
            userResponse.CountryOrigin = user.CountryOrigin;
            userResponse.Citizenship = user.Citizenship;
            userResponse.WebPage = user.WebPage;
            userResponse.Avatar = user.Avatar;
            userResponse.About = user.About;
            userResponse.DoB = user.DoB;
            userResponse.IsActive = user.IsActive;
            userResponse.AccessFailedCount = user.AccessFailedCount;
            userResponse.LockEnabled = user.LockEnabled;
            userResponse.LockoutDescription = user.LockoutDescription;
            userResponse.ReportsToId = user.ReportsToId;
            userResponse.DateCreated = user.DateCreated;
            return userResponse;
        }

    }
}