using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO.Order;

public class OrderDTO
{
	[Required]
	public string CustomerName { get; set; } = default!;
	public string? Comment { get; set; }
}