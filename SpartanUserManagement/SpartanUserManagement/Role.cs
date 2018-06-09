using System;
namespace SpartanUserManagement
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}