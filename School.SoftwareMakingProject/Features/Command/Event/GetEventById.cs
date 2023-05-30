using MediatR;
using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Event
{
	public class GetEventByIdQuery: IRequest<Domain.DbEntities.Event>
	{
        public Guid id { get; set; }
        public GetEventByIdQuery(Guid id)
        {
            this.id = id;
        }
    }

	public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Domain.DbEntities.Event>
	{
        IEventRepository _eventRepo;
        public GetEventByIdQueryHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }
        public async Task<Domain.DbEntities.Event> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
		{
            var eventObj = await _eventRepo.GetByIdAsync(request.id);

            return eventObj;
		}
	}
}
