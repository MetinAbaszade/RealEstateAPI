using AutoMapper;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Implementations;

public class AuthService(IMapper mapper,
                         IUserRepository userRepository,
                         ITokenRepository tokenRepository,
                         ITokenService tokenService) : IAuthService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<string?> GetUserSaltAsync(string contactNumber)
    {
        return await _userRepository.GetUserSaltAsync(contactNumber);
    }

    public async Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var data = await _userRepository.GetAsync(m => m.ContactNumber == dto.ContactNumber &&
                                                          m.Password == dto.Password);

        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserResponseDto>> LoginByTokenAsync()
    {
        var userId = _tokenService.GetUserIdFromToken();

        var data = await _userRepository.GetAsync(m => m.Id == userId);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutAsync(string accessToken)
    {
        var tokens = await _tokenRepository.GetActiveTokensAsync(accessToken);
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutRemovedUserAsync(Guid userId)
    {
        var tokens = (await _tokenRepository.GetListAsync(m => m.UserId == userId)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }
}