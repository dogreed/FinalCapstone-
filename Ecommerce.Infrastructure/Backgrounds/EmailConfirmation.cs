using Ecommerce.Application.IService.IAuthServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Backgrounds
{
	public class EmailConfirmation
	{
		private readonly IEmailService _emailService;

		public EmailConfirmation(IEmailService emailService)
		{
			_emailService = emailService;
		}

		public Task SendVerificationEmail(string toEmail, string subject, string body)
		{
			return _emailService.SendEmail(toEmail, subject, body);
		}
	}
}
