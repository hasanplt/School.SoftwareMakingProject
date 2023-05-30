using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.SoftwareMakingProject.Features.Command.Auth;

namespace School.SoftwareMakingProject.ControllerAPIs
{
	[AllowAnonymous]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		IMediator _mediator;
        public AuthController(IMediator mediator)
        {
			_mediator = mediator;            
        }
        [Route("/api/Auth/Register")]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterUserCommand command)
		{
			var response = await _mediator.Send(command);

			return Ok(response);
		}
		[Route("/api/Auth/Login")]
		[HttpPost]
		public async Task<IActionResult> Login(LoginUserCommand command)
		{
			var response = await _mediator.Send(command);

			return Ok(response);
		}


	}
}
