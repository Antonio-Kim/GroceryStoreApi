using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.Models;

public class Order
{
    [Key]
    [Required]
    public Guid OrderId { get; set; }
    [Required]
    public Guid CartId { get; set; }
    [Required]
    [MaxLength(50)]
    public string? CustomerName { get; set; }
    [Required]
    [MaxLength(200)]
    public string? Comment { get; set; }
}