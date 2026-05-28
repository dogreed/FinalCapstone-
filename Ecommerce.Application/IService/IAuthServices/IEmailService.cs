using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.IService.IAuthServices
{
	public interface IEmailService
	{
		Task SendEmail(string toEmail, string subject, string body);
	}
}
