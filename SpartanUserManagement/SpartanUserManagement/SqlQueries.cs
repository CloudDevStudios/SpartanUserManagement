
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
        public static string GetRoles_Sql = @"SELECT [Id],[RoleName],[IsActive] FROM [Roles];";
        public static string GetRolesById_Sql = @"SELECT [Id], [RoleName], [IsActive] FROM [Roles] WHERE [Id] = @Id;";
        public static string GetRolesByName_Sql = @"SELECT [Id], [RoleName], [IsActive] FROM [Roles] WHERE [RoleName] = @Rolename;";
        public static string DeleteRoleById_Sql = @"DELETE FROM [Roles] WHERE [Id] = @Id;";
        public static string UpdateRole_Sql = @"UPDATE [Roles] SET [RoleName] = @RoleName, [IsActive] = @IsActive WHERE	[Id] = @Id;";
        public static string AddRole_Sql = @"INSERT INTO [Roles] ([Id],[RoleName],[IsActive]) VALUES (@Id, @RoleName, @IsActive );";
        public static string DeleteRoles_Sql = @"DELETE FROM Roles";
    }

}