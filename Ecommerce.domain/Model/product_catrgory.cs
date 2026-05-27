using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.domain.Model
{
	public class product_catrgory
	{
		public int categoryIdid { get; set; }
		[Required (ErrorMessage = "Category Name is Required")]
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public DateTime Created_at { get; set; }
		public DateTime? Updated_at { get; set; }
		public DateTime? Deleted_at { get; set; }


	}
}
