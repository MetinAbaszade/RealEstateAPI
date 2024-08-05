using CORE.Utility;
using DTO.Auth;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace CORE.Abstract;

public interface IUserService
{
    Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<IEnumerable<UserResponseDto>>> GetListAsync();

    Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id);

    Task<IDataResult<User>?> GetAsync(Expression<Func<User, bool>> filter);

    Task<IResult> AddAsync(UserCreateRequestDto dto);

    Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);

    Task<IResult> SetImageAsync(Guid id, string? image = null);

    Task<IDataResult<string>> GetImageAsync(Guid id);

    Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequestDto dto);
}