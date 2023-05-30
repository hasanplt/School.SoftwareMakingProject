using School.SoftwareMakingProject.Helpers.Encryptions;
using MediatR;
using School.SoftwareMakingProject.Domain.DbEntities;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Helpers.Encryptions;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Auth
{
	public class RegisterUserCommand: IRequest<IApiResponse>
	{
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string telNo { get; set; }
        public string tcNo { get; set; }
        public string address { get; set; }
    }

	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IApiResponse>
	{
		IUserRepository _userRepo;
        public RegisterUserCommandHandler(IUserRepository _userRepo)
        {
            this._userRepo = _userRepo;
        }
        public async Task<IApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{

            User user = new User()
            {
                Id = Guid.NewGuid(),    
                Ad = request.firstname,
                Soyad = request.lastname,
                Kullanici_Adi = request.username,
                e_mail = request.email,
                Sifre = EncryptionHelper.ComputeSha256Hash(request.password),
                Telefon_No = request.telNo,
                TC_NO = request.tcNo,
                Adres = request.address,
                Kullanici_Tipi = 0,
            };

            var isExistUser = await _userRepo.GetByParameter("Kullanici_Adi", user.Kullanici_Adi);

            if (isExistUser != null)
                return new ApiResponse<bool>(false, false, "Kullanıcı Zaten Var!");

            var response = await _userRepo.InsertAsync(user);

            return response ? new ApiResponse<bool>(true, true, "Başarıyla Kullanıcı Kayıt Edildi!") : new ApiResponse<bool>(false, false, "Bir Sıkıntı İle Karşılaşıldı!");
		}
	}
}
