using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
	public int CurrentStock { get; set; }
	[Required]
	public bool InStock { get; set; }
}