using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpartanUserManagement
{
    public interface IUserManagement
    {
        Task<List<UserResponse>> GetActiveUsers();
        Task DeleteAllUsers(); // only works in "Development" env.
        Task<UserResponse> GetUserById(Guid id);
        Task<UserResponse> GetUserByUserName(string username);
        Task<UserResponse> GetUserByEmail(string email);
        Task<UserResponse> AddOrUpdateUserWithUserName(User user);
        Task<UserResponse> AddOrUpdateUserWithEmail(User user);
        Task<UserResponse> SetActiveState(Guid id, string description, bool isActive);
        Task<UserResponse> SetLockState(Guid id, string description, bool isLocked);
        Task<UserResponse> ResetPassword(string email, string currentpassword, string confirmedpassword);
        Task<UserResponse> ResetPassword(ResetPassword resetpassword);
        Task<UserResponse> DeleteUserAccount(Guid id, string description);
        Task<UserResponse> LoginByEmail(string email, string password);
        Task<UserResponse> LoginByEmail(LoginEmail loginemail);


    }
}
