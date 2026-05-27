using Ecommerce.Application.IService.IAuthServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string CreateToken(string userId, string email, IList<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userId),
				new Claim(ClaimTypes.Email, email)
			};

			foreach(var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			var key = new SymmetricSecurityKey
					(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
			var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
				issuer: _configuration["Jwt:ValidIssuer"],
				audience: _configuration["Jwt:ValidAudience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(1),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
				ValidateLifetime = false, // We want to get claims from expired token
				ValidIssuer = _configuration["Jwt:ValidIssuer"],
				ValidAudience = _configuration["Jwt:ValidAudience"]
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Invalid token");
			}
			return principal;
		}
	}
}
