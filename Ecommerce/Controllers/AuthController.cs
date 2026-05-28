using Ecommerce.Application.Dtos.AuthDtos;
using Ecommerce.Application.IService.IAuthServices;
using Microsoft.AspNetCore.Authorization;

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
		public async Task<IActionResult> RefreshToken(TokenRequest request)
		{
			var response = await _authService.RefreshToken(request);
			return Ok(response);

		}

		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> Logout(LogoutDto dto)
		{
			await _authService.RevokeToken(dto);
			return Ok(new { Message = "Log ou Sucesfully" });
		}
		[HttpGet("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			var result = await _authService.confirmEmail(userId, token);
			if (result)
			{
				return Ok(new { Message = "Email confirmed successfully" });
			}
			else
			{
				return BadRequest(new { Message = "Invalid token or user ID" });
			}
		}
	}
}
