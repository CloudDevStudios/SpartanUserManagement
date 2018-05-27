//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;

//namespace SpartanUserManagement
//{
//	public class UserAttributes
//	{
//		#region Properties
//		public Guid Id { get; set; }
//		public Guid UserId { get; set; }
//		public string Key { get; set; }
//		public string Value { get; set; }
//		public DateTime DateCreated { get; set; }
//		public DateTime LastUpdated { get; set; }
//		#endregion

//		#region Add
//		/// <summary>
//		/// Adds a new record.
//		/// </summary>
//		public void Add()
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				String sql =  @"INSERT INTO [UserAttributes]
//								(
//									[Id],
//									[UserId],
//									[Key],
//									[Value],
//									[DateCreated],
//									[LastUpdated]
//								)
//								VALUES
//								(
//									@Id,
//									@UserId,
//									@Key,
//									@Value,
//									@DateCreated,
//									@LastUpdated
//								);";

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
//					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
//					cmd.Parameters.Add("@Key", SqlDbType.NVarChar, 100).Value = Key;
//					cmd.Parameters.Add("@Value", SqlDbType.NVarChar, -1).Value = Value;
//					cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime2, 8).Value = DateCreated;
//					cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime2, 8).Value = LastUpdated;
//					cmd.ExecuteNonQuery();
//				}

//				con.Close();
//			}
//		}
//		#endregion

//		#region Update
//		/// <summary>
//		/// Updates an existing record.
//		/// </summary>
//		public void Update()
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				String sql =  @"UPDATE	[UserAttributes]
//								SET		[UserId] = @UserId,
//										[Key] = @Key,
//										[Value] = @Value,
//										[DateCreated] = @DateCreated,
//										[LastUpdated] = @LastUpdated
//								WHERE	[Id] = @Id;";

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
//					cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
//					cmd.Parameters.Add("@Key", SqlDbType.NVarChar, 100).Value = Key;
//					cmd.Parameters.Add("@Value", SqlDbType.NVarChar, -1).Value = Value;
//					cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime2, 8).Value = DateCreated;
//					cmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime2, 8).Value = LastUpdated;
//					cmd.ExecuteNonQuery();
//				}

//				con.Close();
//			}
//		}
//		#endregion

//		#region Delete
//		/// <summary>
//		/// Deletes an existing record.
//		/// </summary>
//		public static void Delete(Guid id)
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				String sql =  @"DELETE	FROM [UserAttributes]
//								WHERE	[Id] = @Id;";

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;
//					cmd.ExecuteNonQuery();
//				}

//				con.Close();
//			}
//		}
//		#endregion

//		#region Get
//		/// <summary>
//		/// Gets an existing record.
//		/// </summary>
//		public static UserAttributes Get(Guid id)
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				String sql =  @"SELECT	[Id],
//										[UserId],
//										[Key],
//										[Value],
//										[DateCreated],
//										[LastUpdated]
//								FROM	[UserAttributes]
//								WHERE	[Id] = @Id;";

//				UserAttributes userAttributes = new UserAttributes();

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

//					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
//					{
//						if (reader.Read())
//						{
//							userAttributes.Id = (Guid)reader["Id"];
//							userAttributes.UserId = (Guid)reader["UserId"];
//							userAttributes.Key = reader["Key"].ToString();
//							userAttributes.Value = reader["Value"].ToString();
//							userAttributes.DateCreated = Convert.ToDateTime(reader["DateCreated"]);
//							userAttributes.LastUpdated = Convert.ToDateTime(reader["LastUpdated"]);
//						}
//					}
//				}

//				return userAttributes;
//			}
//		}
//		#endregion

//		#region GetAll
//		/// <summary>
//		/// Gets all records.
//		/// </summary>
//		public static DataTable GetAll()
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				DataTable dataTable = new DataTable();

//				con.Open();

//				String sql =  @"SELECT	[Id],
//										[UserId],
//										[Key],
//										[Value],
//										[DateCreated],
//										[LastUpdated]
//								FROM	[UserAttributes];";

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
//					{
//						dataTable.Load(reader);
//					}
//				}

//				return dataTable;
//			}
//		}
//		#endregion
//	}
//}