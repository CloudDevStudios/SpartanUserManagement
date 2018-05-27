using System;
using SpartanLogging;
using SpartanSettings;
using SpartanEnvironment;
using SpartanExtensions.Strings;
using SpartanExtensions.Objects;

namespace SpartanUserManagement
{
    public class UserManagementApi  
    {
        private static ISetting _setting;
        private static ILogging _logging;
        private static IEnviroment _env;
        private static string _configPath;
        public string Environment { get; set; }
        public string ConnectionString { get; set; }
        public string DevConnectionString { get; set; }
        public string StagingConnectionString { get; set; }
        public int Timeout { get; set; } = 20;
        public string EncryptKey { get; set; }

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
    }
}
