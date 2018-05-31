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
        Task<UserResponse> AddUserWithUserName(User user);
        Task<UserResponse> AddUserWithEmail(User user);
        Task<UserResponse> DisableUserAccount(Guid id);
        Task<UserResponse> LockUserAccount(Guid id, string description);

        void UpdateUser(User user);       
        void DeleteUserById(Guid id);


    }
}
