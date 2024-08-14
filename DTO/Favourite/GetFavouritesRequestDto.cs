namespace DTO.Favourite;

public record GetFavouritesRequestDto()
{
    public required Guid UserId { get; set; }
    public required int PageIndex { get; set; }
    public required int PageSize { get; set; }
}

