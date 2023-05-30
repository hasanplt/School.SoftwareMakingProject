
using HSchool.SoftwareMakingProject.Helpers.JWT.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Helpers.Encryptions;
using School.SoftwareMakingProject.Helpers.JWT;
using School.SoftwareMakingProject.Persistence.Interfaces;
using System.Security.Principal;

namespace School.SoftwareMakingProject.Features.Command.Auth
{
	public class LoginUserCommand: IRequest<IApiResponse>
	{
        public string username { get; set; }
        public string password { get; set; }
    }

	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IApiResponse>
	{
		IUserRepository _userRepo;
		HttpContext _context;
		public LoginUserCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepo)
		{
			_context = httpContextAccessor.HttpContext;
			_userRepo = userRepo;
        }
        public async Task<IApiResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var isExistUser = await _userRepo.GetByParameters(
				new string[] { "Kullanici_Adi", "Sifre" },
				new string[] { request.username, EncryptionHelper.ComputeSha256Hash(request.password)});

			if(isExistUser == null) 
				return new ApiResponse<bool>(false, false, "Giriş Başarısız!");

			JwtTokenBuilder tokenBuilder = new JwtTokenBuilder()
					.AddIssuer("ProjectExam")
					.AddAudience("ProjectExam")
					.AddSubject("ProjectExam")
					.AddClaim("UserId", isExistUser.Id.ToString())
					.AddSecurityKey(JwtSecurityKey.Create(ConfigurationAppManager._config.GetSection("PrivateTokenKey").Value));

			JwtToken token = tokenBuilder.Build();

			_context.Session.SetString("Token", token.Value);

			return new ApiResponse<bool>(true, true, "Giriş Başarılı! Yönlendiriliyorsunuz...");
		}
	}

}
