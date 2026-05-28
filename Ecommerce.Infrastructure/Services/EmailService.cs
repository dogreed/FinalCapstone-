using Ecommerce.Application.Dtos.AuthDtos;
using Ecommerce.Application.IService.IAuthServices;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net.Mail;

namespace Ecommerce.Infrastructure.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _settings;
		public EmailService(IOptions<EmailSettings> settings)
		{
			_settings = settings.Value;
		}
		public async Task SendEmail(string toEmail, string subject, string body)
		{
			using var client = new SmtpClient(_settings.Host, _settings.Port)
			{
				Credentials = new System.Net.NetworkCredential(_settings.Email, _settings.Password),
				EnableSsl = true
			};
			var mailMessage = new MailMessage
			{
				From = new MailAddress(_settings.Email, _settings.FromName),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};
			mailMessage.To.Add(toEmail);
			await client.SendMailAsync(mailMessage);


		}
	}
}
