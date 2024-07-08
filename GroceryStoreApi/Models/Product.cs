using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreApi.Models;

[Table("Products")]
public class Product
{
	[Key]
	[Required]
	public int Id { get; set; }
	[Required]
	[MaxLength(50)]
	public string? Category { get; set; }
	[Required]
	[MaxLength(100)]
	public string? Name { get; set; }
	[Required]
	[MaxLength]
	public string? Manufacturer { get; set; }
	[Required]
	[Precision(6, 2)]
	public decimal Price { get; set; }
	[Required]
	[JsonPropertyName("current_stock")]
	public int CurrentStock { get; set; }

	public ICollection<Transactions>? Transactions { get; set; }
}