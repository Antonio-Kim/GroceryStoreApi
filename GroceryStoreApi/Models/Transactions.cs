using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryStoreApi.Models;

public class Transactions
{
	[Key]
	[Required]
	[Column(Order = 0)]
	public Guid CartId { get; set; }
	[Key]
	[Required]
	[Column(Order = 1)]
	public int ProductId { get; set; }
	[Required]
	public int Quantity { get; set; }

	public Cart? Cart { get; set; }
	public Product? Product { get; set; }
}