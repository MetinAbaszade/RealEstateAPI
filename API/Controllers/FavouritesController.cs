using API.Attributes;
using BLL.Implementations;
using CORE.Abstract;
using DTO.Favourite;
using DTO.Property;
using DTO.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
[ApiController]
public class FavouritesController(IFavouriteService favouriteService,
                                  ITokenService tokenService) : Controller
{
    [SwaggerOperation(Summary = "add favourite")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("add/{propertyId}")]
    public async Task<IActionResult> AddAsync([FromRoute] int propertyId)
    {
        var userId = tokenService.GetUserIdFromToken();
        var response = await favouriteService.AddAsync(userId, propertyId);
        return response is ErrorResult ? NotFound(response) : Ok(response);
    }

    [SwaggerOperation(Summary = "delete favourite")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("delete/{propertyId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int propertyId)
    {
        var userId = tokenService.GetUserIdFromToken();
        var response = await favouriteService.DeleteAsync(userId, propertyId);
        return response is ErrorResult ? NotFound(response) : Ok(response);
    }

    [SwaggerOperation(Summary = "get favourites")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<GetPropertyResponseDto>>))]
    [HttpGet("{pageIndex}/{pageSize}")]
    public async Task<IActionResult> GetFavourites([FromRoute] int pageIndex, int pageSize)
    {
        var userId = tokenService.GetUserIdFromToken();
        var response = await favouriteService.GetFavouritesAsync(new() { UserId = userId,
                                                                                                                           PageIndex = pageIndex,
                                                                                                                           PageSize = pageSize });
        return response is ErrorDataResult<List<GetPropertyResponseDto>> ? NotFound(response) : Ok(response);
    }
}
