using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace School.SoftwareMakingProject.Controllers
{
	public class AuthController : Controller
	{
        HttpContext _context;
        public AuthController(IHttpContextAccessor accessor)
        {
            _context = accessor.HttpContext;
        }
        [AllowAnonymous]
		[Route("/auth")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

        [Authorize]
        [Route("/logout")]
        [HttpGet]
        public async Task<IActionResult> logout()
        {
            _context.Session.Remove("Token");
            return Redirect("/auth");
        }
    }
}
