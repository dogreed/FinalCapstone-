
using System.ComponentModel.DataAnnotations;


namespace Ecommerce.domain.Model
{
	public class discount
	{
		public int discountId { get; set; }
		[Required(ErrorMessage = "Discount Name is Required")]
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public decimal discountPercentage { get; set; }

		public bool active { get; set; } 
		public DateTime Created_at { get; set; } = DateTime.Now;
		public DateTime? Updated_at { get; set; }
		public DateTime? Deleted_at { get; set; }
		

	}
}
