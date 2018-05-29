using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpartanUserManagement
{
    public interface IUserManagement
    {
        Task<List<UserResponse>> GetAllUsers();
        Task DeleteAllUsers(); // only works in "Development" env.
        Task<UserResponse> GetUserById(Guid id);
        Task<UserResponse> GetUserByUserName(string username);
        Task<UserResponse> GetUserByEmail(string email);
        Task<UserResponse> AddUserByUserName(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid id);


    }
}
