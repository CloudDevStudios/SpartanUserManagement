using System;
using System.Collections.Generic;

namespace SpartanUserManagement

{
    public class User
    {
        #region Properties
        public Guid Id { get; set; }
        public Guid RowId { get; set; }
        public Guid UpdateId { get; set; }
        public string AppName { get; set; }
        public string AppTheme { get; set; }
        public string AppSetting { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
        public string ShortCuts { get; set; }
        public string PasswordHash { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }     
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }
        public string EmailSignature { get; set; }
        public string EmailProvider { get; set; }
        public string JobTitle { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string FaxNumber { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CountryOrigin { get; set; }
        public string Citizenship { get; set; }
        public string WebPage { get; set; }
        public string Avatar { get; set; }
        public string About { get; set; }
        public DateTime? DoB { get; set; }
        public bool IsActive { get; set; }
        public short AccessFailedCount { get; set; }
        public bool LockEnabled { get; set; }
        public string LockoutDescription { get; set; }
        public string AccountNotes { get; set; }
        public Guid? ReportsToId { get; set; }
        public DateTime DateCreated { get; set; }
        public IList<User> Users { get; set; } = new List<User>();
        #endregion
    }
}