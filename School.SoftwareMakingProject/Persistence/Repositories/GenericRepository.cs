using Dapper;
using School.SoftwareMakingProject.Persistence.Interfaces;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace School.SoftwareMakingProject.Persistence.Repositories
{
	public class GenericRepository<T>: MSSQLConnection, IGenericRepository<T> where T : class
	{
		public string _tableName = typeof(T).Name;
		public GenericRepository(string tableName)
		{
			_tableName = tableName;
		}

		public GenericRepository()
		{

		}

		public async Task<List<T>> GetAllByParameter(string parameter, string value)
		{
			return (await _conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE {parameter} = '{value}'")).ToList();
		}

		public async Task<T> GetByParameter(string parameter, string value)
		{
			return (await _conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE {parameter} = '{value}'")).FirstOrDefault();
		}

		public async Task<T> GetByParameters(string[] parameters, string[] values)
		{
			string query = GenerateWhereQueryByParametersAndValues(parameters, values);
			return (await _conn.QueryAsync<T>(query)).FirstOrDefault();
		}

		public async Task<List<T>> GetAllByParameters(string[] parameters, string[] values)
		{
			string query = GenerateWhereQueryByParametersAndValues(parameters, values);
			return (await _conn.QueryAsync<T>(query)).ToList();
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			try
			{
				await _conn.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool DeleteSync(Guid id)
		{
			try
			{
				_conn.Execute($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public List<T> GetAllSync()
		{
			return (_conn.Query<T>($"SELECT * FROM {_tableName}")).ToList();
		}

		public async Task<List<T>> GetAllAsync()
		{
			return (await _conn.QueryAsync<T>($"SELECT * FROM {_tableName}")).ToList();
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return (await _conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE id = '{id}'")).FirstOrDefault();
		}

		public T GetByIdSync(Guid id)
		{
			return (_conn.Query<T>($"SELECT * FROM {_tableName} WHERE id = '{id}'")).FirstOrDefault();
		}

		public async Task<bool> InsertAsync(T entity)
		{
			try
			{
				string query = GenerateInsertQuery();
				await _conn.ExecuteAsync(query, entity);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool InsertSync(T entity)
		{
			try
			{
				string query = GenerateInsertQuery();
				_conn.Execute(query, entity);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> UpdateAsync(T entity)
		{
			try
			{
				var updateQuery = GenerateUpdateQuery();
				await _conn.ExecuteAsync(updateQuery, entity);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool UpdateSync(T entity)
		{
			try
			{
				var updateQuery = GenerateUpdateQuery();
				_conn.Execute(updateQuery, entity);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		public string GenerateInsertQuery()
		{
			var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

			insertQuery.Append("(");

			var properties = GenerateListOfProperties(GetProperties.ToList());
			properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

			insertQuery
				.Remove(insertQuery.Length - 1, 1)
				.Append(") VALUES (");

			properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

			insertQuery
				.Remove(insertQuery.Length - 1, 1)
				.Append(")");

			return insertQuery.ToString();
		}
		public string GenerateUpdateQuery()
		{
			var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
			var properties = GenerateListOfProperties(GetProperties.ToList());

			properties.ForEach(property =>
			{
				if (!property.Equals("id"))
				{
					updateQuery.Append($"{property}=@{property},");
				}
			});

			updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
			updateQuery.Append(" WHERE id=@id");

			return updateQuery.ToString();
		}
		public string GenerateWhereQueryByParametersAndValues(string[] parameters, string[] values)
		{
			var whereQuery = new StringBuilder($"SELECT * FROM {_tableName} ");

			whereQuery.Append("WHERE ");

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters.Length == (i + 1))
				{
					whereQuery.Append($"{parameters[i]} = '{values[i]}'");
					break;
				}
				whereQuery.Append($"{parameters[i]} = '{values[i]}' and ");
			}

			return whereQuery.ToString();
		}

		public static List<string> GenerateListOfProperties(List<PropertyInfo> listOfProperties)
		{
			return (from prop in listOfProperties
					let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
					where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
					select prop.Name).ToList();
		}


		public IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

	}
}
