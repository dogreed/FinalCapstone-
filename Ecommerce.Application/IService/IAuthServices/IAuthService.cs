using Ecommerce.Application.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.IService.IAuthServices
{
	public interface IAuthService
	{
		Task Register(RegisterRequest request);
		Task<AuthResponse> Login(LoginDto request);
		Task<AuthResponse> RefreshToken(string refreshToken);
	}
}
