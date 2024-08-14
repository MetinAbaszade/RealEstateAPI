using API.Attributes;
using BLL.Implementations;
using CORE.Abstract;
using DTO.Property;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class PropertiesController(IPropertyService propertyService) : ControllerBase
{

    [SwaggerOperation(Summary = "get properties as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<Property>>))]
    [HttpGet]
    public async Task<IActionResult> GetAsPaginated([FromQuery] GetPropertiesRequestDto dto)
    {
        var response = await propertyService.GetAsPaginatedListAsync(dto);
        return Ok(response);
    }
}
