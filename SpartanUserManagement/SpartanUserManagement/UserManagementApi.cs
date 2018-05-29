using System;
using SpartanLogging;
using SpartanSettings;
using SpartanEnvironment;
using SpartanExtensions.Strings;
using SpartanExtensions.Objects;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace SpartanUserManagement
{
    public class UserManagementApi  : IUserManagement
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
        public async Task<List<UserResponse>> GetAllUsers()
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

                        _users = db.Query<UserResponse>(SqlQueries.GetAllUsers_Sql).ToList();
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

                        _user = db.QueryFirst<User>(SqlQueries.GetUserById_Sql, new {Id = id });
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

                        _user = db.QueryFirst<User>(SqlQueries.GetUserByUserName_Sql, new { UserName = username });
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

                        _user = db.QueryFirst<User>(SqlQueries.GetUserByEmail_Sql, new { Email = email });
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
        /// UserName is a requirement.
        /// Email is a requirement.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserResponse</returns>
        public async Task<UserResponse> AddUserByUserName(User user)
        {
            
            var _userResponse = new UserResponse();
            var _userTemp = new UserResponse();
            int _rowsAffected = 0;
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
            if (!user.Email.IsValidEmail())
                return ResponseError_ModelView(_errorTile, "Invalid Email");

            _userTemp = await GetUserByEmail(user.Email);
            if (_userTemp.Status.Equals("ok") && !string.IsNullOrWhiteSpace(_userTemp.Email))
                return ResponseError_ModelView(_errorTile, "Email already exist");

            //Verify if User exist by email
            if(user.Id != Guid.Empty)
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
                try
                {
                    using (IDbConnection db = new SqlConnection(ConnectionString))
                    {
                        if (db.State == ConnectionState.Closed)                    
                            db.Open();
                        
                        user.Id = (user.Id == Guid.Empty) ? Guid.NewGuid() : user.Id;
                        user.PasswordHash = user.PasswordHash.EncryptString(EncryptKey);
                        user.IsActive = true;
                        user.AccessFailedCount = 0;
                        user.LockEnabled = false;
                        user.LockoutDescription = string.Empty;
                        user.ReportsToId = null;
                        user.DateCreated = DateTime.Now;
                        user.LastUpdated = user.DateCreated;

                        _rowsAffected = db.Execute(SqlQueries.AddUser_Sql, new {
                            user.Id,
                            user.AppName,
                            user.UserName,
                            user.PasswordHash,
                            user.Type,
                            user.Company,
                            user.FirstName,
                            user.MiddleName,
                            user.LastName,
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
                            user.WebPage,
                            user.Avatar,
                            user.About,
                            user.DoB,
                            user.IsActive,
                            user.AccessFailedCount,
                            user.LockEnabled,
                            user.LockoutDescription,                            
                            user.ReportsToId,
                            user.DateCreated,
                            user.LastUpdated
                        });
                    }

                    //Make sure execute fuction suceeded
                    if (_rowsAffected <= 0)
                        return ResponseError_ModelView(_errorTile, $"No records were inserted. Row Affected were {_rowsAffected}");

                    //Only show ModelView response
                    _userResponse = ResponseOk_ModelView(user);

                }
                catch (Exception ex)
                {
                    _logging.Error("UserManagementApi:GetUserById", ex.ToString());
                }

                return _userResponse;
            });
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


        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserById(Guid id)
        {
            throw new NotImplementedException();
        }


        public string GetPasswordValidationPhrase(string value)
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
            userResponse.FirstName = user.FirstName;
            userResponse.MiddleName = user.MiddleName;
            userResponse.LastName = user.LastName;
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
            userResponse.WebPage = user.WebPage;
            userResponse.Avatar = user.Avatar;
            userResponse.About = user.About;
            userResponse.DoB = user.DoB;
            userResponse.ReportsToId = user.ReportsToId;
            return userResponse;
        }

    }
}
