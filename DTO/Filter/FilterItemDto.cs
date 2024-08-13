namespace DTO.Search;

public record FilterItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Keyword { get; set; } = default!;
}
