using MediatR;
using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Query.User
{
    public class GetAuthUserQuery: IRequest<Domain.DbEntities.User>
    {
    }

    public class GetAuthUserQueryHandler : IRequestHandler<GetAuthUserQuery, Domain.DbEntities.User>
    {
        HttpContext _context;
        IUserRepository _userRepo;
        public GetAuthUserQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _context = httpContextAccessor.HttpContext;
        }
        public async Task<Domain.DbEntities.User> Handle(GetAuthUserQuery request, CancellationToken cancellationToken)
        {
            string userId = _context.User.Claims.Where(x => x.Type.Equals("UserId")).FirstOrDefault().Value;

            var user = await _userRepo.GetByIdAsync(Guid.Parse(userId));

            return user;
        }
    }
}
