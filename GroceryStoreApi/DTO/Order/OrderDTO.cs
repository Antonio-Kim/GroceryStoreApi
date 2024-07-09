using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO.Order;

public class OrderDTO
{
	public string? CustomerName { get; set; }
	public string? Comment { get; set; }
}