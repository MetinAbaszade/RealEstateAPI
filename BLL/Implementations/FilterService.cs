using AutoMapper;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework;
using DAL.EntityFramework.GenericRepository;
using DTO.Responses;
using DTO.Search;
using ENTITIES.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Implementations;

public class FilterService(IServiceProvider serviceProvider,
                           IMapper mapper) : IFilterService
{

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetBuildingTypesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<BuildingType>>();
        var buildingTypes = await repository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(buildingTypes), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetCurrenciesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Currency>>();
        var currencies = await repository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(currencies), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetDocumentsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var documentRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Document>>();
        var documents = await documentRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(documents), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetOperationTypesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var operationTypeRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<OperationType>>();
        var operationTypes = await operationTypeRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(operationTypes), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetOwnerTypesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var ownerTypeRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<OwnerType>>();
        var ownerTypes = await ownerTypeRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(ownerTypes), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetPropertyTypesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var propertyTypeRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<PropertyType>>();
        var propertyTypes = await propertyTypeRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(propertyTypes), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetRegionsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var regionRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Region>>();
        var regions = await regionRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(regions), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetRepairRatesAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var repairRateRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<RepairRate>>();
        var repairRates = await repairRateRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(repairRates), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetRoomCountsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var roomCountRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<RoomCount>>();
        var roomCounts = await roomCountRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(roomCounts), EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<FilterItemDto>>> GetTargetsAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var targetRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Target>>();
        var targets = await targetRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<FilterItemDto>>(mapper.Map<IEnumerable<FilterItemDto>>(targets), EMessages.Success.Translate());
    }
}
