using MediatR;
using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Event
{
	public class GetAllEventByUser: IRequest<IApiResponse>
	{
	}

	public class GetAllEventByUserHandler : IRequestHandler<GetAllEventByUser, IApiResponse>
	{
		IEventRepository _eventRepo;
		HttpContext _context;
        public GetAllEventByUserHandler(IHttpContextAccessor httpContextAccessor, IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
			_context = httpContextAccessor.HttpContext;
        }
        public async Task<IApiResponse> Handle(GetAllEventByUser request, CancellationToken cancellationToken)
		{
			string userId = _context.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault().Value;

			var events = await _eventRepo.GetAllByParameter("UserId", userId);

			return new ApiResponse<List<Domain.DbEntities.Event>>(events, true, "Başarılı.");
		}
	}
}
