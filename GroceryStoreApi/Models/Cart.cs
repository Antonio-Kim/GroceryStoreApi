using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.Models;

public class Cart
{
	[Required]
	public Guid CartId { get; set; }
	[Required]
	public DateTime Created { get; set; }

	public ICollection<Transactions>? Transactions { get; set; }
}