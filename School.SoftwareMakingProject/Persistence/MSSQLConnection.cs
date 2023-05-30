using System.Data.SqlClient;

namespace School.SoftwareMakingProject.Persistence
{
	public class MSSQLConnection
	{
		public SqlConnection _conn;
		public MSSQLConnection()
		{
			_conn = new SqlConnection(ConfigurationAppManager._config.GetConnectionString("MSSQLConnection"));
		}
	}
}
