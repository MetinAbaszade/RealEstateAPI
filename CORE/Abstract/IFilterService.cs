using DTO.Responses;
using DTO.Search;

namespace CORE.Abstract;
public interface IFilterService
{
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetBuildingTypesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetDocumentsAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetOperationTypesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetOwnerTypesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetPropertyTypesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetRegionsAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetRepairRatesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetRoomCountsAsync();

    Task<IDataResult<IEnumerable<FilterItemDto>>> GetCurrenciesAsync();
    Task<IDataResult<IEnumerable<FilterItemDto>>> GetTargetsAsync();
}
