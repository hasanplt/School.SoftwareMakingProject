namespace School.SoftwareMakingProject.Persistence.Interfaces
{
	public interface IGenericRepository<T> : IRepository where T : class
	{
		#region SYNC
		bool DeleteSync(Guid id);
		List<T> GetAllSync();
		T GetByIdSync(Guid id);
		bool InsertSync(T entity);
		bool UpdateSync(T entity);
		#endregion

		#region ASYNC
		Task<bool> UpdateAsync(T entity);
		Task<bool> InsertAsync(T entity);
		Task<List<T>> GetAllAsync();
		Task<T> GetByIdAsync(Guid id);
		Task<bool> DeleteAsync(Guid id);
		Task<List<T>> GetAllByParameter(string parameter, string value);
		Task<T> GetByParameter(string parameter, string value);
		Task<T> GetByParameters(string[] parameters, string[] values);
		Task<List<T>> GetAllByParameters(string[] parameters, string[] values);
		#endregion
	}
}
