//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;

//namespace SpartanUserManagement
//{
//	public class Menus
//	{
//		#region Properties
//		public Guid Id { get; set; }
//		public Guid? ReportsToId { get; set; }
//		public string MenuText { get; set; }
//		public string MenuURL { get; set; }
//		public int? SortOrder { get; set; }
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
//				String sql =  @"INSERT INTO [Menus]
//								(
//									[Id],
//									[ReportsToId],
//									[MenuText],
//									[MenuURL],
//									[SortOrder]
//								)
//								VALUES
//								(
//									@Id,
//									@ReportsToId,
//									@MenuText,
//									@MenuURL,
//									@SortOrder1
//								);";

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
//					cmd.Parameters.Add("@ReportsToId", SqlDbType.UniqueIdentifier).Value = ReportsToId == null ? (Object)DBNull.Value : ReportsToId;
//					cmd.Parameters.Add("@MenuText", SqlDbType.NVarChar, 100).Value = MenuText;
//					cmd.Parameters.Add("@MenuURL", SqlDbType.NVarChar, 400).Value = MenuURL;
//					cmd.Parameters.Add("@SortOrder1", SqlDbType.Int, 4).Value = SortOrder == null ? (Object)DBNull.Value : SortOrder;
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
//				String sql =  @"UPDATE	[Menus]
//								SET		[ReportsToId] = @ReportsToId,
//										[MenuText] = @MenuText,
//										[MenuURL] = @MenuURL,
//										[SortOrder] = @SortOrder1
//								WHERE	[Id] = @Id;";

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
//					cmd.Parameters.Add("@ReportsToId", SqlDbType.UniqueIdentifier).Value = ReportsToId == null ? (Object)DBNull.Value : ReportsToId;
//					cmd.Parameters.Add("@MenuText", SqlDbType.NVarChar, 100).Value = MenuText;
//					cmd.Parameters.Add("@MenuURL", SqlDbType.NVarChar, 400).Value = MenuURL;
//					cmd.Parameters.Add("@SortOrder1", SqlDbType.Int, 4).Value = SortOrder == null ? (Object)DBNull.Value : SortOrder;
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
//				String sql =  @"DELETE	FROM [Menus]
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
//		public static Menus Get(Guid id)
//		{
//			String connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;

//			using (SqlConnection con = new SqlConnection(connectionString))
//			{
//				String sql =  @"SELECT	[Id],
//										[ReportsToId],
//										[MenuText],
//										[MenuURL],
//										[SortOrder]
//								FROM	[Menus]
//								WHERE	[Id] = @Id;";

//				Menus menus = new Menus();

//				con.Open();

//				using (SqlCommand cmd = new SqlCommand(sql, con))
//				{
//					cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

//					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
//					{
//						if (reader.Read())
//						{
//							menus.Id = (Guid)reader["Id"];
//							menus.ReportsToId = reader["ReportsToId"] == DBNull.Value ? (Guid?)null : (Guid)reader["ReportsToId"];
//							menus.MenuText = reader["MenuText"].ToString();
//							menus.MenuURL = reader["MenuURL"].ToString();
//							menus.SortOrder = reader["SortOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["SortOrder"]);
//						}
//					}
//				}

//				return menus;
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
//										[ReportsToId],
//										[MenuText],
//										[MenuURL],
//										[SortOrder]
//								FROM	[Menus];";

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