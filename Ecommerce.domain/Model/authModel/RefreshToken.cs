

namespace Ecommerce.domain.Model.authModel
{
	public class RefreshToken
	{
		public int Id { get; set; }
		public string Token { get; set; } = string.Empty;
		public DateTime Expires { get; set; }
		public bool IsRevoked { get; set; }
		public string UserId { get; set; }
	
		public DateTime Created { get; set; } = DateTime.UtcNow;


	}
}
