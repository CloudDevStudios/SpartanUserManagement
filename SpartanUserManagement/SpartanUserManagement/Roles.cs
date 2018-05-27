using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SpartanUserManagement
{
	public class Roles
	{
		#region Properties
		public Guid Id { get; set; }
		public string RoleName { get; set; }
		public bool IsActive { get; set; }
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
				String sql =  @"INSERT INTO [Roles]
								(
									[Id],
									[RoleName],
									[IsActive]
								)
								VALUES
								(
									@Id,
									@RoleName,
									@IsActive
								);";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = RoleName;
					cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = IsActive;
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
				String sql =  @"UPDATE	[Roles]
								SET		[RoleName] = @RoleName,
										[IsActive] = @IsActive
								WHERE	[Id] = @Id;";

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
					cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = RoleName;
					cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 1).Value = IsActive;
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
				String sql =  @"DELETE	FROM [Roles]
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
		public static Roles Get(Guid id)
		{
			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				String sql =  @"SELECT	[Id],
										[RoleName],
										[IsActive]
								FROM	[Roles]
								WHERE	[Id] = @Id;";

				Roles roles = new Roles();

				con.Open();

				using (SqlCommand cmd = new SqlCommand(sql, con))
				{
					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						if (reader.Read())
						{
							roles.Id = (Guid)reader["Id"];
							roles.RoleName = reader["RoleName"].ToString();
							roles.IsActive = Convert.ToBoolean(reader["IsActive"]);
						}
					}
				}

				return roles;
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
										[RoleName],
										[IsActive]
								FROM	[Roles];";

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