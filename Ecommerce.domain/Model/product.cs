using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.domain.Model
{
	public class product
	{
		public int product_id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public string SKU { get; set; } = string.Empty;
		public int categoryId { get; set; }
		public int inventoryId { get; set; }
		

		public DateTime Created_at { get; set; } = DateTime.Now;
		public DateTime? Updated_at { get; set; }
		public DateTime? Deleted_at { get; set; }
	}
}
