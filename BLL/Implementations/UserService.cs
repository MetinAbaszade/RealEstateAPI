using AutoMapper;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Localization;
using CORE.Utility;
using DAL.EntityFramework.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace BLL.Implementations;

public class UserService(IMapper mapper,
                         IUserRepository userRepository,
                         ITokenRepository tokenRepository) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;

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

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _userRepository.GetAsync(id);

        await _userRepository.SoftDeleteAsync(data!);

        var tokens = (await _tokenRepository.GetListAsync(m => m.UserId == id)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SetImageAsync(Guid id, string? image = null)
    {
        var user = await _userRepository.GetAsync(id);
        user.Image = image;

        await _userRepository.UpdateAsync(user);
        return new SuccessResult();
    }

    public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetListAsync()
    {
        var datas = await _userRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<UserResponseDto>>(_mapper.Map<IEnumerable<UserResponseDto>>(datas), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<UserByIdResponseDto>(await _userRepository.GetAsync(m => m.Id == id));
        return new SuccessDataResult<UserByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IDataResult<User>> GetAsync(Expression<Func<User, bool>> filter)
    {
        var user = await _userRepository.GetAsync(filter);
        if (user is null)
        {
            return new ErrorDataResult<User>(EMessages.UserIsNotExist.Translate());
        }
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
        var getUserResult = await GetAsync(u => u.ContactNumber == contactNumber);
        if (getUserResult is not SuccessDataResult<User>)
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        var user = getUserResult.Data;
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

    public async Task<IDataResult<string>> GetImageAsync(Guid id)
    {
        var user = await _userRepository.GetAsNoTrackingAsync(u => u.Id == id);
        if (user is { Image: not null })
        {
            return new SuccessDataResult<string>(user.Image, EMessages.Success.Translate());
        }
        return new SuccessDataResult<string>(EMessages.FileIsNotFound.Translate());
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