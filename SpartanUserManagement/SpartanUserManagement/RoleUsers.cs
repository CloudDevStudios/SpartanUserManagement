using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SpartanUserManagement
{
	public class RoleUsers
	{
		#region Properties
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public int RoleId { get; set; }
		public Guid? Role_RoleIdId { get; set; }
		#endregion

		#region Add
		/// <summary>
		/// Adds a new record.
		/// </summary>
		public void Add()
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"INSERT INTO [RoleUsers]
								(
									[Id],
									[UserId],
									[RoleId],
									[Role_RoleIdId]
								)
								VALUES
								(
									@Id,
									@UserId,
									@RoleId,
									@Role_RoleIdId
								);";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
					cmd.Parameters.Add("@RoleId", SqlDbType.Int, 4).Value = RoleId;
					cmd.Parameters.Add("@Role_RoleIdId", SqlDbType.UniqueIdentifier).Value = Role_RoleIdId == null ? (Object)DBNull.Value : Role_RoleIdId;
					cmd.ExecuteNonQuery();
				}

				con.Close();
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates an existing record.
		/// </summary>
		public void Update()
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"UPDATE	[RoleUsers]
								SET		[UserId] = @UserId,
										[RoleId] = @RoleId,
										[Role_RoleIdId] = @Role_RoleIdId
								WHERE	[Id] = @Id;";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
					cmd.Parameters.Add("@RoleId", SqlDbType.Int, 4).Value = RoleId;
					cmd.Parameters.Add("@Role_RoleIdId", SqlDbType.UniqueIdentifier).Value = Role_RoleIdId == null ? (Object)DBNull.Value : Role_RoleIdId;
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
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"DELETE	FROM [RoleUsers]
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

		#region Get
		/// <summary>
		/// Gets an existing record.
		/// </summary>
		public static RoleUsers Get(Guid id)
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"SELECT	[Id],
										[UserId],
										[RoleId],
										[Role_RoleIdId]
								FROM	[RoleUsers]
								WHERE	[Id] = @Id;";

				RoleUsers roleUsers = new RoleUsers();

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						if (reader.Read())
						{
							roleUsers.Id = (Guid)reader["Id"];
							roleUsers.UserId = (Guid)reader["UserId"];
							roleUsers.RoleId = Convert.ToInt32(reader["RoleId"]);
							roleUsers.Role_RoleIdId = reader["Role_RoleIdId"] == DBNull.Value ? (Guid?)null : (Guid)reader["Role_RoleIdId"];
						}
					}
				}

				return roleUsers;
			}
		}
		#endregion

		#region GetAll
		/// <summary>
		/// Gets all records.
		/// </summary>
		public static DataTable GetAll()
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				DataTable dataTable = new DataTable();

				con.Open();

				String sql =  @"SELECT	[Id],
										[UserId],
										[RoleId],
										[Role_RoleIdId]
								FROM	[RoleUsers];";

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						dataTable.Load(reader);
					}
				}

				return dataTable;
			}
		}
		#endregion
	}
}