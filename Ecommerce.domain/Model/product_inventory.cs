using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.domain.Model
{
	public class product_inventory
	{


		public int inventoryId { get; set; }
		[Required(ErrorMessage = "Quantity is required")]
		public int quantity { get; set; }
		public DateTime Created_at { get; set; } = DateTime.Now;
		public DateTime? Updated_at { get; set; }
		public DateTime? Deleted_at { get; set; }

	}
}
