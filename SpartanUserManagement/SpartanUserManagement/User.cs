using System;
using System.Data;
using System.Data.SqlClient;

namespace SpartanUserManagement

{
    public class User
    {
        #region Properties
        public Guid Id { get; set; }
        public string AppName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
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
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string WebPage { get; set; }
        public string Avatar { get; set; }
        public string About { get; set; }
        public DateTime? DoB { get; set; }
        public bool IsActive { get; set; }
        public short AccessFailedCount { get; set; }
        public bool LockEnabled { get; set; }
        public string LockoutDescription { get; set; }
        public Guid? ReportsToId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        #endregion


        #region Update
        /// <summary>
        /// Updates an existing record.
        /// </summary>
        public void Update()
        {
            String connectionString = "";//ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                String sql = @"UPDATE	[Users]
								SET		[AppName] = @AppName,
										[UserName] = @UserName,
										[PasswordHash] = @PasswordHash,
										[Type] = @Type,
										[Company] = @Company,
										[FirstName] = @FirstName,
										[MiddleName] = @MiddleName,
										[LastName] = @LastName,
										[Gender] = @Gender,
										[MaritalStatus] = @MaritalStatus,
										[Email] = @Email,
										[EmailSignature] = @EmailSignature,
										[EmailProvider] = @EmailProvider,
										[JobTitle] = @JobTitle,
										[BusinessPhone] = @BusinessPhone,
										[HomePhone] = @HomePhone,
										[MobilePhone] = @MobilePhone,
										[FaxNumber] = @FaxNumber,
										[Address] = @Address,
										[Address1] = @Address1,
										[City] = @City,
										[State] = @State,
										[Province] = @Province,
										[ZipCode] = @ZipCode,
										[Country] = @Country,
										[WebPage] = @WebPage,
										[Avatar] = @Avatar,
										[About] = @About,
										[DoB] = @DoB,
										[IsActive] = @IsActive,
										[AccessFailedCount] = @AccessFailedCount,
										[LockEnabled] = @LockEnabled,
										[LockoutDescription] = @LockoutDescription,
										[ReportsToId] = @ReportsToId,
										[DateCreated] = @DateCreated,
										[LastUpdated] = @LastUpdated
								WHERE	[Id] = @Id;";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
                    cmd.Parameters.Add("@AppName", SqlDbType.NVarChar, 30).Value = AppName == null ? (Object)DBNull.Value : AppName;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 10).Value = UserName == null ? (Object)DBNull.Value : UserName;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, -1).Value = PasswordHash;
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 20).Value = Type == null ? (Object)DBNull.Value : Type;
                    cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 50).Value = Company == null ? (Object)DBNull.Value : Company;
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 30).Value = FirstName == null ? (Object)DBNull.Value : FirstName;
                    cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 30).Value = MiddleName == null ? (Object)DBNull.Value : MiddleName;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 30).Value = LastName == null ? (Object)DBNull.Value : LastName;
                    cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 10).Value = Gender == null ? (Object)DBNull.Value : Gender;
                    cmd.Parameters.Add("@MaritalStatus", SqlDbType.NVarChar, 20).Value = MaritalStatus == null ? (Object)DBNull.Value : MaritalStatus;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
                    cmd.Parameters.Add("@EmailSignature", SqlDbType.NVarChar, 100).Value = EmailSignature == null ? (Object)DBNull.Value : EmailSignature;
                    cmd.Parameters.Add("@EmailProvider", SqlDbType.NVarChar, 20).Value = EmailProvider == null ? (Object)DBNull.Value : EmailProvider;
                    cmd.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 50).Value = JobTitle == null ? (Object)DBNull.Value : JobTitle;
                    cmd.Parameters.Add("@BusinessPhone", SqlDbType.NVarChar, 15).Value = BusinessPhone == null ? (Object)DBNull.Value : BusinessPhone;
                    cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 15).Value = HomePhone == null ? (Object)DBNull.Value : HomePhone;
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar, 15).Value = MobilePhone == null ? (Object)DBNull.Value : MobilePhone;
                    cmd.Parameters.Add("@FaxNumber", SqlDbType.NVarChar, 15).Value = FaxNumber == null ? (Object)DBNull.Value : FaxNumber;
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = Address == null ? (Object)DBNull.Value : Address;
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = Address1 == null ? (Object)DBNull.Value : Address1;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 20).Value = City == null ? (Object)DBNull.Value : City;
                    cmd.Parameters.Add("@State", SqlDbType.NVarChar, 20).Value = State == null ? (Object)DBNull.Value : State;
                    cmd.Parameters.Add("@Province", SqlDbType.NVarChar, 30).Value = Province == null ? (Object)DBNull.Value : Province;
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 10).Value = ZipCode == null ? (Object)DBNull.Value : ZipCode;
                    cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 50).Value = Country == null ? (Object)DBNull.Value : Country;
                    cmd.Parameters.Add("@WebPage", SqlDbType.NVarChar, 400).Value = WebPage == null ? (Object)DBNull.Value : WebPage;
                    cmd.Parameters.Add("@Avatar", SqlDbType.NVarChar, 400).Value = Avatar == null ? (Object)DBNull.Value : Avatar;
                    cmd.Parameters.Add("@About", SqlDbType.NVarChar, 400).Value = About == null ? (Object)DBNull.Value : About;
                    cmd.Parameters.Add("@DoB", SqlDbType.DateTime2, 8).Value = DoB == null ? (Object)DBNull.Value : DoB;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = IsActive;
                    cmd.Parameters.Add("@AccessFailedCount", SqlDbType.SmallInt, 2).Value = AccessFailedCount;
                    cmd.Parameters.Add("@LockEnabled", SqlDbType.Bit, 1).Value = LockEnabled;
                    cmd.Parameters.Add("@LockoutDescription", SqlDbType.NVarChar, 400).Value = LockoutDescription == null ? (Object)DBNull.Value : LockoutDescription;
                    cmd.Parameters.Add("@ReportsToId", SqlDbType.UniqueIdentifier).Value = ReportsToId == null ? (Object)DBNull.Value : ReportsToId;
                    cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime2, 8).Value = DateCreated;
                    cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime2, 8).Value = LastUpdated;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes an existing record.
        /// </summary>
        public static void Delete(Guid id)
        {
            String connectionString = "";//ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                String sql = @"DELETE	FROM [Users]
								WHERE	[Id] = @Id;";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        #endregion


    }
}