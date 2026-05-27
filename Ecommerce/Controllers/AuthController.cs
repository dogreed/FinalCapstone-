using Ecommerce.Application.Dtos.AuthDtos;
using Ecommerce.Application.IService.IAuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterRequest request)
		{
			await _authService.Register(request);
			return Ok(new { Message = "User Created Successfully" });
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDto request)
		{
			var response = await _authService.Login(request);
			return Ok(response);
		}
		[HttpPost("ref")]
		public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
		{
			var response = await _authService.RefreshToken(refreshToken);
			return Ok(response);

		}
	}
}
