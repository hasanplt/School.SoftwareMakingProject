using MediatR;
using School.SoftwareMakingProject.Domain.Responses;
using School.SoftwareMakingProject.Persistence.Interfaces;

namespace School.SoftwareMakingProject.Features.Command.Event
{
    public class UpdateEventCommand: IRequest<IApiResponse>
    {
        public Guid id { get; set; }
        public string description { get; set; }
        public DateTime eventdatetime { get; set; }
    }

    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, IApiResponse>
    {
        IEventRepository _eventRepo;
        public UpdateEventCommandHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }
        public async Task<IApiResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventobg = await _eventRepo.GetByIdAsync(request.id);

            if(eventobg == null)
            {
                return new ApiResponse<bool>(false, false, "Bulunamadı!");
            }

            eventobg.description = request.description;
            eventobg.event_end_datetime = request.eventdatetime;
            
            bool isSuccessUpdate = await _eventRepo.UpdateAsync(eventobg);

            if (isSuccessUpdate)
                return new ApiResponse<bool>(true, true, "Başarılı");
            else
                return new ApiResponse<bool>(false, false, "Başarısız");
        }
    }
}
