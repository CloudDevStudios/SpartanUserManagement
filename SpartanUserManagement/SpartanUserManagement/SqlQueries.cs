
namespace SpartanUserManagement
{
    public class SqlQueries
    {
        public static string GetUserById_Sql = "User_ById";
        public static string GetUserByUserName_Sql = "User_ByUserName";
        public static string GetUserByEmail_Sql = "User_ByEmail";
        public static string GetActiveUsers_Sql = "User_GetActiveUsers";    
        public static string DeleteAllUsers_Sql = @"DELETE FROM USERS"; //Only for Development and UnitTesting
        public static string UserAddOrUpdate_Sql = "User_InsertOrUpdate";
    }

}