using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.AuthDtos
{
	public class TokenRequest
	{
		public string AccessToken { get; set; }
		public string RefereshToken { get; set; }
	}
}
