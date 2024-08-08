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
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IResult> AddAsync(UserCreateRequestDto dto)
    {

        var userExists = await _userRepository.IsUserExistbyContactNumberAsync(dto.ContactNumber);
        if (userExists)
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        var data = _mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        await _userRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var data = await _userRepository.GetAsync(id);

        await _userRepository.DeleteAsync(data!);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetListAsync()
    {
        var datas = await _userRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<UserResponseDto>>(_mapper.Map<IEnumerable<UserResponseDto>>(datas), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null)
            return new ErrorDataResult<UserByIdResponseDto>(EMessages.UserIsExist.Translate());
        var data = _mapper.Map<UserByIdResponseDto>(await _userRepository.GetAsync(id));
        return new SuccessDataResult<UserByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IDataResult<User>?> GetAsync(Expression<Func<User, bool>> filter)
    {
        var user = await _userRepository.SingleOrDefaultAsync(filter);
        if (user is null)
            return new ErrorDataResult<User>(EMessages.UserIsExist.Translate());
        return new SuccessDataResult<User>(user, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto)
    {
        if (await _userRepository.IsUserExistbyContactNumberAsync(dto.ContactNumber))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        var data = _mapper.Map<User>(dto);
        data.Id = id;

        await _userRepository.UpdateUserAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateVerifiedStatusAsync(string contactNumber, bool isVerified)
    {
        var user = await _userRepository.SingleOrDefaultAsync(u => u.ContactNumber == contactNumber);
        if (user is null)
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        user.Verified = isVerified;

        await _userRepository.UpdateUserAsync(user);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _userRepository.GetList();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<UserResponseDto>(_mapper.Map<List<UserResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<UserResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IResult> ResetPasswordAsync(Guid id, string password)
    {
        var data = await _userRepository.GetAsync(id);

        if (data is null)
        {
            return new ErrorResult(EMessages.UserIsNotExist.Translate());
        }

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(password, data.Salt);

        await _userRepository.UpdateAsync(data);

        return new SuccessResult(EMessages.PasswordResetted.Translate());
    }
}