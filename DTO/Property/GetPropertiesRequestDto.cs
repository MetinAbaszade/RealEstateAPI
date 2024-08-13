using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Property;
public record GetPropertiesRequestDto()
{
    public required int PageIndex { get; set; }
    public required int PageSize { get; set; }
    public int? BuildingTypeId { get; set; }
    public int? DocumentId { get; set; }
    public int? OperationTypeId { get; set; }
    public int? OwnerTypeId { get; set; }
    public int? PropertyTypeId { get; set; }
    public int? RegionId { get; set; }
    public int? RoomCountId { get; set; }
}
