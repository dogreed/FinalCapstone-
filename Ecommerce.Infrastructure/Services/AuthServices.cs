using Ecommerce.Application.Dtos.AuthDtos;
using Ecommerce.Application.IService.IAuthServices;
using Ecommerce.domain.Model.authModel;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security.Claims;

namespace Ecommerce.Application.Services
{
	public class AuthServices : IAuthService
	{
		private readonly UserManager<AppIdentityUser> _userManager;
		private readonly ITokenService _tokenService;
		public AuthServices(UserManager<AppIdentityUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		public async Task Register(RegisterRequest request)
		{
			var checkUser = await _userManager.FindByEmailAsync(request.Email);
			if (checkUser != null)
			{
				throw new Exception("User already exists");
			}
			var user = new AppIdentityUser
			{
				FullName = request.FullName,
				Email = request.Email,
				UserName = request.Email
			};
			var result = await _userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded)
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				throw new Exception($"User creation failed: {errors}");
			}
			await _userManager.AddToRoleAsync(user, "User");
		}


		public async Task<AuthResponse> Login(LoginDto request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
			{
				throw new Exception("Invalid email or password");
			}
			var roles = await _userManager.GetRolesAsync(user);
			var accessToken = _tokenService.CreateToken(user.Id, user.Email, roles);
			var refreshToke = _tokenService.GenerateRefreshToken();
			user.RefreshTokens.Add(new RefreshToken
			{
				Token = refreshToke,
				Expires = DateTime.UtcNow.AddDays(7),
				IsRevoked = false,
				UserId = user.Id,
			});

			await _userManager.UpdateAsync(user);
			return new AuthResponse
			{
				AccessToken = accessToken,
				RefreshToken = refreshToke
			};
		}

		public async Task<AuthResponse> RefreshToken(string refreshToken)
		{
			var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken);
			var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new Exception("Invalid refresh token");
			}
			var storedToken = user.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
			if (storedToken == null || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
			{
				throw new Exception("Invalid refresh token");
			}
			storedToken.IsRevoked = true;
			var roles = await _userManager.GetRolesAsync(user);
			var newAccessToken = _tokenService.CreateToken(
							   user.Id,
							   user.Email,
							   roles
						   );
			var refereshtoken = _tokenService.GenerateRefreshToken();

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = _tokenService.GenerateRefreshToken(),
				Expires = DateTime.UtcNow.AddDays(7),
				IsRevoked = false,
				UserId = user.Id,
			});
			await _userManager.UpdateAsync(user);

			return new AuthResponse
			{
				AccessToken = newAccessToken,
				RefreshToken = refereshtoken
			};
		}
	}
}
