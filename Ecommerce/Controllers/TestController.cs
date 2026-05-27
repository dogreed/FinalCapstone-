using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class TestController : ControllerBase
	{
		public async Task<IActionResult> Get()
		{
			return Ok(new { Message = "This is a protected endpoint" });
		}
	}
}
