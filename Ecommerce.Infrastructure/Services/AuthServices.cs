using Ecommerce.Application.Dtos.AuthDtos;
using Ecommerce.Application.IService.IAuthServices;
using Ecommerce.domain.Model.authModel;
using Ecommerce.Infrastructure.Backgrounds;
using Ecommerce.Infrastructure.Identity;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text;

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
				UserName = request.Email,
				EmailConfirmed = false
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				throw new Exception($"User creation failed: {errors}");
			}

			// assign role
			await _userManager.AddToRoleAsync(user, "User");

			// IMPORTANT: await token
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			// encode safely
			var encoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

			// build link
			var confirmationLink =
				$"https://localhost:7071/api/auth/ConfirmEmail?userId={user.Id}&token={encoded}";

			// send email
			BackgroundJob.Enqueue<EmailConfirmation>(job =>
			job.SendVerificationEmail(user.Email, "Confirm Email", $"Click here: <a href='{confirmationLink}'>Verify Email</a>"));
		}


		public async Task<AuthResponse> Login(LoginDto request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
			{
				throw new Exception("Invalid email or password");
			}
			if (!user.EmailConfirmed)
			{
				throw new Exception("Please verify your email first");
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

		public async Task<AuthResponse> RefreshToken(TokenRequest token)
		{
			var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
			var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.Users
										 .Include(u => u.RefreshTokens)
										 .FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				throw new Exception("Invalid refresh token");
			}
			var storedToken = user.RefreshTokens.FirstOrDefault(t => t.Token == token.RefreshToken);
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
			var newRefreshToken = _tokenService.GenerateRefreshToken();

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = newRefreshToken,
				Expires = DateTime.UtcNow.AddDays(7),
				IsRevoked = false,
				UserId = user.Id,
			});
			await _userManager.UpdateAsync(user);

			return new AuthResponse
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken
			};
		}

		public async Task RevokeToken(LogoutDto dto)
		{
			var user = await _userManager.Users
	   .Include(u => u.RefreshTokens)
	   .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == dto.token));

			if (user == null) return;

			var token = user.RefreshTokens
				.SingleOrDefault(t => t.Token == dto.token);

			if (token == null || token.IsRevoked) return;

			token.IsRevoked = true;

			await _userManager.UpdateAsync(user);
		}

		public async Task<bool> confirmEmail(string userId, string token)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found");
			}
			var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
			var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
			if (!result.Succeeded)
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				throw new Exception($"Email confirmation failed: {errors}");
			}
			return true;
		}
	}
}
