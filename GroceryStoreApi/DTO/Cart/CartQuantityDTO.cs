using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO.Cart;

public class CartQuantityDTO
{
    [Required]
    [DefaultValue(1)]
    public int Quantity { get; set; } = 1;
}