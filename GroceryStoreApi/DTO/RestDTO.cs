namespace GroceryStoreApi.DTO;

public class RestDTO<T>
{
    public T Data { get; set; } = default!;
    public int Results { get; set; }
    public string? Category { get; set; }
    public string? Available { get; set; }
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
}