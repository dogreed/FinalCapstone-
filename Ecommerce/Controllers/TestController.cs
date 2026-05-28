using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "User")]
	public class TestController : ControllerBase
	{
		public async Task<IActionResult> Get()
		{
			return Ok(new { Message = "This is a protected endpoint where user can see product" });
		}


	}
}
