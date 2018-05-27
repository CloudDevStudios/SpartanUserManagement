using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SpartanUserManagement
{
	public class MenuPermissions
	{
		#region Properties
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid MenuId { get; set; }
		public Guid RoleId { get; set; }
		public int? SortOrder { get; set; }
		public bool? IsCreate { get; set; }
		public bool? IsRead { get; set; }
		public bool? IsUpdate { get; set; }
		public bool? IsDelete { get; set; }
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
				String sql =  @"INSERT INTO [MenuPermissions]
								(
									[Id],
									[UserId],
									[MenuId],
									[RoleId],
									[SortOrder],
									[IsCreate],
									[IsRead],
									[IsUpdate],
									[IsDelete]
								)
								VALUES
								(
									@Id,
									@UserId,
									@MenuId,
									@RoleId,
									@SortOrder1,
									@IsCreate,
									@IsRead,
									@IsUpdate,
									@IsDelete
								);";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
					cmd.Parameters.Add("@MenuId", SqlDbType.UniqueIdentifier).Value = MenuId;
					cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = RoleId;
					cmd.Parameters.Add("@SortOrder1", SqlDbType.Int, 4).Value = SortOrder == null ? (Object)DBNull.Value : SortOrder;
					cmd.Parameters.Add("@IsCreate", SqlDbType.Bit, 1).Value = IsCreate == null ? (Object)DBNull.Value : IsCreate;
					cmd.Parameters.Add("@IsRead", SqlDbType.Bit, 1).Value = IsRead == null ? (Object)DBNull.Value : IsRead;
					cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit, 1).Value = IsUpdate == null ? (Object)DBNull.Value : IsUpdate;
					cmd.Parameters.Add("@IsDelete", SqlDbType.Bit, 1).Value = IsDelete == null ? (Object)DBNull.Value : IsDelete;
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
				String sql =  @"UPDATE	[MenuPermissions]
								SET		[UserId] = @UserId,
										[MenuId] = @MenuId,
										[RoleId] = @RoleId,
										[SortOrder] = @SortOrder1,
										[IsCreate] = @IsCreate,
										[IsRead] = @IsRead,
										[IsUpdate] = @IsUpdate,
										[IsDelete] = @IsDelete
								WHERE	[Id] = @Id;";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
					cmd.Parameters.Add("@MenuId", SqlDbType.UniqueIdentifier).Value = MenuId;
					cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = RoleId;
					cmd.Parameters.Add("@SortOrder1", SqlDbType.Int, 4).Value = SortOrder == null ? (Object)DBNull.Value : SortOrder;
					cmd.Parameters.Add("@IsCreate", SqlDbType.Bit, 1).Value = IsCreate == null ? (Object)DBNull.Value : IsCreate;
					cmd.Parameters.Add("@IsRead", SqlDbType.Bit, 1).Value = IsRead == null ? (Object)DBNull.Value : IsRead;
					cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit, 1).Value = IsUpdate == null ? (Object)DBNull.Value : IsUpdate;
					cmd.Parameters.Add("@IsDelete", SqlDbType.Bit, 1).Value = IsDelete == null ? (Object)DBNull.Value : IsDelete;
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
				String sql =  @"DELETE	FROM [MenuPermissions]
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
		public static MenuPermissions Get(Guid id)
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"SELECT	[Id],
										[UserId],
										[MenuId],
										[RoleId],
										[SortOrder],
										[IsCreate],
										[IsRead],
										[IsUpdate],
										[IsDelete]
								FROM	[MenuPermissions]
								WHERE	[Id] = @Id;";

				MenuPermissions menuPermissions = new MenuPermissions();

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						if (reader.Read())
						{
							menuPermissions.Id = (Guid)reader["Id"];
							menuPermissions.UserId = (Guid)reader["UserId"];
							menuPermissions.MenuId = (Guid)reader["MenuId"];
							menuPermissions.RoleId = (Guid)reader["RoleId"];
							menuPermissions.SortOrder = reader["SortOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SortOrder"]);
							menuPermissions.IsCreate = reader["IsCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsCreate"]);
							menuPermissions.IsRead = reader["IsRead"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsRead"]);
							menuPermissions.IsUpdate = reader["IsUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsUpdate"]);
							menuPermissions.IsDelete = reader["IsDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["IsDelete"]);
						}
					}
				}

				return menuPermissions;
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
										[MenuId],
										[RoleId],
										[SortOrder],
										[IsCreate],
										[IsRead],
										[IsUpdate],
										[IsDelete]
								FROM	[MenuPermissions];";

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