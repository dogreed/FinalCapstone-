using Ecommerce.domain.Model.authModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Identity
{
	public class AppIdentityUser : IdentityUser
	{

		public string FullName { get; set; }
		public List<RefreshToken> RefreshTokens { get; set; } = new();

	}
}
