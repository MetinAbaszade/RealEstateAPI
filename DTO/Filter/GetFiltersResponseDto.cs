namespace DTO.Search;

public record GetFiltersResponseDto()
{
    public required IEnumerable<FilterItemDto> BuildingTypes { get; set; }
    public required IEnumerable<FilterItemDto> Documents { get; set; }
    public required IEnumerable<FilterItemDto> OperationTypes { get; set; }
    public required IEnumerable<FilterItemDto> OwnerTypes { get; set; }
    public IEnumerable<FilterItemDto> PropertyTypes { get; set; }
    public required IEnumerable<FilterItemDto> Regions { get; set; }
    public IEnumerable<FilterItemDto> RepairRates { get; set; }
    public required IEnumerable<FilterItemDto> RoomCounts { get; set; }
    public required IEnumerable<FilterItemDto> Currencies { get; set; }
    public IEnumerable<FilterItemDto> Targets { get; set; }
}
