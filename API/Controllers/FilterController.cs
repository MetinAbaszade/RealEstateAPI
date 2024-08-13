using API.Attributes;
using BLL.Implementations;
using CORE.Abstract;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using DTO.Search;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
[ApiController]
public class FilterController(IFilterService searchService) : Controller
{
    [SwaggerOperation(Summary = "get all filter datas")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<GetFiltersResponseDto>))]
    [HttpGet("filters")]
    public async Task<IActionResult> GetFilters()
    {
        var searchTasks = new List<Task<IDataResult<IEnumerable<FilterItemDto>>>>
        {
            searchService.GetBuildingTypesAsync(),
            searchService.GetCurrenciesAsync(),
            searchService.GetDocumentsAsync(),
            searchService.GetOperationTypesAsync(),
            searchService.GetOwnerTypesAsync(),
            searchService.GetPropertyTypesAsync(),
            searchService.GetRegionsAsync(),
            searchService.GetRepairRatesAsync(),
            searchService.GetRoomCountsAsync(),
            searchService.GetTargetsAsync()
        };

        var results = await Task.WhenAll(searchTasks);

        if (results.Any(result => result is not SuccessDataResult<IEnumerable<FilterItemDto>>))
        {
            return BadRequest(EMessages.DataNotFound.Translate());
        }

        // Construct the response DTO
        var searchItemResponseDto = new GetFiltersResponseDto
        {
            BuildingTypes = results[0].Data,
            Currencies = results[1].Data,
            Documents = results[2].Data,
            OperationTypes = results[3].Data,
            OwnerTypes = results[4].Data,
            PropertyTypes = results[5].Data,
            Regions = results[6].Data,
            RepairRates = results[7].Data,
            RoomCounts = results[8].Data,
            Targets = results[9].Data
        };
        return Ok(searchItemResponseDto);
    }
}
