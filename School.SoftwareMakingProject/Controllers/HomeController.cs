using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.SoftwareMakingProject.Features.Command.Event;
using School.SoftwareMakingProject.Features.Query.User;
using System.Diagnostics;

namespace School.SoftwareMakingProject.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		IMediator _mediator;
        public HomeController(IMediator mediator)
        {
			_mediator = mediator;
        }

        [Route("/calendar")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var user = await _mediator.Send(new GetAuthUserQuery());
			return View("Index", user);
		}

		[Route("/calendar/add")]
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			return View();
		}


		[Route("/calendar/edit/{id}")]
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var eventObj = await _mediator.Send(new GetEventByIdQuery(id));
			
			if (eventObj == null)
				return Redirect("/calendar");

			return View("Edit", eventObj);
		}
	}
}