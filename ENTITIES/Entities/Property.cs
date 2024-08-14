namespace ENTITIES.Entities;

public class Property
{
    public int IdProperty { get; set; }

    public int? FkIdSource { get; set; }

    public int? FkIdLink { get; set; }

    public string? Code { get; set; }

    public int? FkIdPropertyType { get; set; }

    public int? FkIdOperationType { get; set; }

    public int? FkIdCity { get; set; }

    public string? Address { get; set; }

    public int? FkIdDocument { get; set; }

    public double? Price { get; set; }

    public int? FkIdCurrency { get; set; }

    public string? Data { get; set; }

    public double? Area { get; set; }

    public double? GeneralArea { get; set; }

    public int? Floor { get; set; }

    public int? FloorOf { get; set; }

    public int? FkIdRoom { get; set; }

    public int? FkIdBuildingType { get; set; }

    public double? UnitPrice { get; set; }

    public string? EX { get; set; }

    public string? EY { get; set; }

    public string? CpName { get; set; }

    public string? CpPhoneNumber01 { get; set; }

    public string? CpPhoneNumber02 { get; set; }

    public string? CpPhoneNumber03 { get; set; }

    public int? FkIdOwnerType { get; set; }

    public string? Images { get; set; }

    public DateTime? InsertDate { get; set; }

    public int? UploadStatus { get; set; }

    public string? UploadMessage { get; set; }

    public int? FkIdMetro { get; set; }

    public int? ApprovmentStatus { get; set; }

    public string? ApprovmentMessage { get; set; }

    public int? FkIdRepair { get; set; }

    public int? FkIdTarget { get; set; }

    public virtual ICollection<Favourite> Favourites { get; set; } = [];
}

