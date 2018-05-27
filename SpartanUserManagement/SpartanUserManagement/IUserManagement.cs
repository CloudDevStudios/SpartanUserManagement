using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpartanUserManagement
{
    public interface IUserManagement
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse> GetUserById(Guid id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUserById(Guid id);


    }
}
