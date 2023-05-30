using MediatR;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Event
{
	public class DeleteEventCommand: IRequest<IApiResponse>
	{
        public Guid id { get; set; }
        public DeleteEventCommand(Guid id)
        {
			this.id = id;
        }
    }

	public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, IApiResponse>
	{
		IEventRepository _eventRepo;
        public DeleteEventCommandHandler(IEventRepository eventRepo)
        {
			_eventRepo = eventRepo;
        }
        public async Task<IApiResponse> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
		{
			
			var eventObj = await _eventRepo.GetByIdAsync(request.id);

			if (eventObj == null)
				return new ApiResponse<bool>(false, false, "Silenecek Etkinlik Bulunamadı!");

			var isSuccessDelete = await _eventRepo.DeleteAsync((Guid)eventObj.Id);

			if(isSuccessDelete)
				return new ApiResponse<bool>(true, true, "Başarıyla Silindi");
			else
				return new ApiResponse<bool>(false, false, "Silinirken bir hata ile karşılaşıldı!");
		}
	}
}
