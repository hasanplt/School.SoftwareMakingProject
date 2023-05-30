using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Persistence.Repositories
{
	public class UserRepository: GenericRepository<User>, IUserRepository
	{
        public UserRepository(): base("Users")
        {
            
        }
    }
}
