using Ecommerce.domain.Model;
using Ecommerce.domain.Model.authModel;
using Ecommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Infrastructure.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppIdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			
		}
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		public DbSet<product> products { get; set; }
		public DbSet<product_catrgory> product_Catrgories { get; set; }
	}
}
