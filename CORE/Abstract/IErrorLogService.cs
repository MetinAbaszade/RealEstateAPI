using CORE.Utility;
using DTO.ErrorLog;
using DTO.Responses;

namespace CORE.Abstract;

public interface IErrorLogService
{
    Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<IEnumerable<ErrorLogResponseDto>>> GetAsync();

    Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(ErrorLogCreateDto dto);
}