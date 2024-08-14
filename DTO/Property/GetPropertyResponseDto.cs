
namespace DTO.Property;

public record GetPropertyResponseDto()
{
    public int Id { get; set; }
    public int? SourceId { get; set; }
    public int? LinkId { get; set; }
    public string? Code { get; set; }
    public string? PropertyTypeName { get; set; }
    public string? OperationTypeName { get; set; }
    public int? RegionName { get; set; }
    public string? Address { get; set; }
    public string? DocumentName { get; set; }
    public double? Price { get; set; }
    public string? Currency { get; set; }
    public string? Data { get; set; }
    public double? Area { get; set; }
    public double? GeneralArea { get; set; }
    public int? Floor { get; set; }
    public int? FloorOf { get; set; }
    public string? RoomCountName { get; set; }
    public string? BuildingTypeName { get; set; }
    public string? ContactPersonName { get; set; }
    public string? ContactPersonPhoneNumber { get; set; }
    public string? ContactPersonSecondPhoneNumber { get; set; }
    public string? ContactPersonThirdPhoneNumber { get; set; }
    public string? OwnerTypeName { get; set; }
    public string? Images { get; set; }
    public DateTime? InsertDate { get; set; }
    public int? MetroId { get; set; }
    public string? RepairRateName { get; set; }
    public string? TargetName { get; set; }
}
