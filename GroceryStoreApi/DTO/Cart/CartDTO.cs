using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO.Cart;

public class CartDTO
{
	[Required]
	public int productId { get; set; } = default!;
	[Range(1,100)]
	public int quantity { get; set; } = 1;
}