using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO;

public class ProductRequestDTO
{
	[DefaultValue(20)]
	[Range(1, 20)]
	public int Results { get; set; } = 20;
	[DefaultValue(null)]
	public string? Category { get; set; } = null;
	[DefaultValue(null)]
	public string? Available { get; set; } = null;
}