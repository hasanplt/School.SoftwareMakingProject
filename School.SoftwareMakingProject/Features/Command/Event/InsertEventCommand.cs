using MediatR;
using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Event
{
	public class InsertEventCommand: IRequest<IApiResponse>
	{
        public string description { get; set; }
        public DateTime eventdatetime { get; set; }
    }

	public class InsertEventCommandHandler : IRequestHandler<InsertEventCommand, IApiResponse>
	{
		IEventRepository _eventRepo;
		HttpContext _context;
		public InsertEventCommandHandler(IHttpContextAccessor httpContextAccessor, IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
			_context = httpContextAccessor.HttpContext;
		}
        public async Task<IApiResponse> Handle(InsertEventCommand request, CancellationToken cancellationToken)
		{
			string userId = _context.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault().Value;
			var eventData = new Domain.DbEntities.Event()
			{
				Id = Guid.NewGuid(),
				event_end_datetime = request.eventdatetime,
				event_start_datetime = request.eventdatetime,
				created_datetime = DateTime.Now,
				description = request.description,
				is_complete = false,
				is_on_remind = false,
				userId = Guid.Parse(userId),
				typeid = Guid.Parse("4cf7de1b-f537-4181-bb81-2d3798e2a842")
			};

			var response = await _eventRepo.InsertAsync(eventData);

			if (response)
				return new ApiResponse<bool>(true, true, "Başarılı");
			else
				return new ApiResponse<bool>(false, false, "Başarısız");
		}
	}
}
