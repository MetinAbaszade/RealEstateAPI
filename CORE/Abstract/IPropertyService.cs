using CORE.Utility;
using DTO.Property;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace CORE.Abstract;
public interface IPropertyService
{
    Task<IDataResult<List<GetPropertiesResponseDto>>> GetAsPaginatedListAsync(GetPropertiesRequestDto dto);
    Task<IDataResult<Property>> GetAsync(int id);
}
