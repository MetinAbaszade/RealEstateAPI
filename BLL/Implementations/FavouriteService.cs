using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.GenericRepository;
using DTO.Favourite;
using DTO.Property;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Implementations;

public class FavouriteService(IGenericRepository<Favourite> favouriteRepository,
                              IPropertyService propertyService,
                              IUserRepository userRepository) : IFavouriteService
{
    public async Task<IResult> AddAsync(Guid userId, int propertyId)
    {
        var propertyExist = await propertyService.IsPropertyExistbyId(propertyId);
        if (!propertyExist)
        {
            return new ErrorResult(EMessages.DataNotFound.Translate());
        }
        var favouriteExist = await favouriteRepository.AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        if (favouriteExist) {
            return new ErrorResult(EMessages.FavouriteAlreadyExists.Translate());
        }
        var favourite = new Favourite()
        {
            UserId = userId,
            PropertyId = propertyId,
        };
        await favouriteRepository.AddAsync(favourite);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> DeleteAsync(Guid userId, int propertyId)
    {
        var favourite = await favouriteRepository.SingleOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        if (favourite == null)
        {
            return new ErrorResult(EMessages.DataNotFound.Translate());
        }
        await favouriteRepository.DeleteAsync(favourite);
        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<List<GetPropertyResponseDto>>> GetFavouritesAsync(GetFavouritesRequestDto dto)
    {
        var favourites = await favouriteRepository.GetListAsync(f => f.UserId == dto.UserId);
        var properties = new List<GetPropertyResponseDto>();
        foreach (var favourite in favourites)
        {
            var getPropertyResult = await propertyService.GetAsync(favourite.PropertyId);
            if (getPropertyResult is ErrorDataResult<GetPropertyResponseDto>)
            {
                return new ErrorDataResult<List<GetPropertyResponseDto>>(getPropertyResult.Message);
            }
            properties.Add(getPropertyResult.Data);
        }
        return new SuccessDataResult<List<GetPropertyResponseDto>>(properties, EMessages.Success.Translate());
    }
}
