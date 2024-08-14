using DTO.Favourite;
using DTO.Property;
using DTO.Responses;

namespace CORE.Abstract;

public interface IFavouriteService
{
    Task<IResult> AddAsync(Guid userId, int propertyId);
    Task<IResult> DeleteAsync(Guid userId, int propertyId);
    Task<IDataResult<List<GetPropertyResponseDto>>> GetFavouritesAsync(GetFavouritesRequestDto dto);
}
