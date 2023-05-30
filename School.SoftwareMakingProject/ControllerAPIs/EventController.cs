using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.SoftwareMakingProject.Features.Command.Event;

namespace School.SoftwareMakingProject.ControllerAPIs
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventController : ControllerBase
	{
		IMediator _mediator;
        public EventController(IMediator mediator)
        {
			_mediator = mediator;
        }
        [Route("/api/event")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var response = await _mediator.Send(new GetAllEventByUser());

			return Ok(response);
		}
		[Route("/api/event/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await _mediator.Send(new DeleteEventCommand(id));

			return Ok(response);
		}
		[Route("/api/event/update")]
		[HttpGet]
		public async Task<IActionResult> Update([FromQuery]UpdateIsCompleteCommand command)
		{
			var response = await _mediator.Send(command);

			return Ok(response);
		}
		[Route("/api/event/edit")]
		[HttpGet]
		public async Task<IActionResult> Edit([FromQuery]UpdateEventCommand command)
		{
			var response = await _mediator.Send(command);

			return Ok(response);
		}
		[Route("/api/event/insert")]
		[HttpGet]
		public async Task<IActionResult> Insert([FromQuery]InsertEventCommand command)
		{
			var response = await _mediator.Send(command);

			return Ok(response);
		}
	}
}
