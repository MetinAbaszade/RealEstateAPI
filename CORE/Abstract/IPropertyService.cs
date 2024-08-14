using CORE.Utility;
using DTO.Property;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace CORE.Abstract;
public interface IPropertyService
{
    Task<IDataResult<List<GetPropertyResponseDto>>> GetAsPaginatedListAsync(GetPropertiesRequestDto dto);
    Task<IDataResult<GetPropertyResponseDto>> GetAsync(int id);
    Task<bool> IsPropertyExistbyId(int id);
}
