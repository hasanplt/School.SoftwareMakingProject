using MediatR;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Persistence.Interfaces;
using System.ComponentModel.Design;

namespace School.SoftwareMakingProject.Features.Command.Event
{
	public class UpdateIsCompleteCommand: IRequest<IApiResponse>
	{
        public string? id { get; set; }
        public string? is_complete { get; set; }
	}

	public class UpdateIsCompleteCommandHandler : IRequestHandler<UpdateIsCompleteCommand, IApiResponse>
	{
        IEventRepository _eventRepo;
        public UpdateIsCompleteCommandHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }
        public async Task<IApiResponse> Handle(UpdateIsCompleteCommand request, CancellationToken cancellationToken)
		{
            var eventObj = await _eventRepo.GetByIdAsync(Guid.Parse(request.id));
			
			eventObj.is_complete = (request.is_complete == "true");

            bool isSuccessUpdate = await _eventRepo.UpdateAsync(eventObj);

            if (isSuccessUpdate)
				return new ApiResponse<bool>(true, true, "Etkinlik Güncellendi!");
			else
				return new ApiResponse<bool>(true, true, "Etkinlik Güncellenemedi");
		}
	}
}
