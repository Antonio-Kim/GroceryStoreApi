using System.ComponentModel.DataAnnotations;

namespace GroceryStoreApi.DTO;

public class AuthDTO
{
	[Required]
	[MinLength(2)]
	public string ClientName { get; set; } = default!;
	[Required]
	[MinLength(6)]
	public string ClientEmail { get; set; } = default!;
}