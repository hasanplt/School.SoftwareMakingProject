using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Persistence.Repositories
{
	public class EventRepository : GenericRepository<Event>, IEventRepository
	{
        public EventRepository(): base("Events")
        {
            
        }


    }
}
