using DTO.Logging;

namespace CORE.Abstract;

public interface ILoggingService
{
    Task AddLogAsync(RequestLogDto dto);
}