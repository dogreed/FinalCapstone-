using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repository.Product
{
	public class ProductRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;


		public ProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<T> Add(T entity, CancellationToken ct = default)
		{
			await _context.Set<T>().AddAsync(entity);
			await _context.SaveChangesAsync(ct);
			return entity;
		}

		public async Task<IReadOnlyList<T>> Get(T entity, CancellationToken ct = default)
		{
			var result = await _context.Set<T>().AsNoTracking().ToListAsync(ct);
			return result;
		}
	}
}
