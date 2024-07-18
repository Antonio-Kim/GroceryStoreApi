namespace GroceryStoreApi.DTO;

public class ProductsDTO
{
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public bool InStock { get; set; }
}