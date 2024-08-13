using AutoMapper;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Localization;
using CORE.Utility;
using DAL.EntityFramework.Abstract;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace BLL.Implementations;

public class UserService(IMapper mapper,
                         IUserRepository userRepository) : IUserService
{
    public async Task<IResult> AddAsync(UserCreateRequestDto dto)
    {

        var userExists = await userRepository.IsUserExistbyContactNumberAsync(dto.ContactNumber);
        if (userExists)
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        var data = mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        await userRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var data = await userRepository.GetAsync(id);

        await userRepository.DeleteAsync(data!);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetListAsync()
    {
        var datas = await userRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<UserResponseDto>>(mapper.Map<IEnumerable<UserResponseDto>>(datas), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id)
    {
        var user = await userRepository.GetAsync(id);
        if (user == null)
            return new ErrorDataResult<UserByIdResponseDto>(EMessages.UserIsExist.Translate());
        var data = mapper.Map<UserByIdResponseDto>(await userRepository.GetAsync(id));
        return new SuccessDataResult<UserByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IDataResult<User>?> GetAsync(Expression<Func<User, bool>> filter)
    {
        var user = await userRepository.SingleOrDefaultAsync(filter);
        if (user is null)
            return new ErrorDataResult<User>(EMessages.UserIsExist.Translate());
        return new SuccessDataResult<User>(user, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto)
    {
        if (await userRepository.IsUserExistbyContactNumberAsync(dto.ContactNumber))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        var data = mapper.Map<User>(dto);
        data.Id = id;

        await userRepository.UpdateUserAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateVerifiedStatusAsync(string contactNumber, bool isVerified)
    {
        var user = await userRepository.SingleOrDefaultAsync(u => u.ContactNumber == contactNumber);
        if (user is null)
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        user.Verified = isVerified;

        await userRepository.UpdateUserAsync(user);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = userRepository.GetList();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<UserResponseDto>(mapper.Map<List<UserResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<UserResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IResult> ResetPasswordAsync(Guid id, string password)
    {
        var data = await userRepository.GetAsync(id);

        if (data is null)
        {
            return new ErrorResult(EMessages.UserIsNotExist.Translate());
        }

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(password, data.Salt);

        await userRepository.UpdateAsync(data);

        return new SuccessResult(EMessages.PasswordResetted.Translate());
    }
}