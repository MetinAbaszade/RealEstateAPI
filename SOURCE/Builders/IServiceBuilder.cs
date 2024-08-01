﻿using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IServiceBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities
            .Where(w =>
                w.Options.BuildService
                && w.Options.BuildDto
                && w.Options.BuildRepository)
            .ToList()
            .ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.I_SERVICE_PATH, $"I{model.Name}Service.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using DAL.EntityFramework.Utility;
                   using DTO.Responses;
                   using DTO.{entityName};

                   namespace BLL.Abstract;

                   public interface I{entityName}Service
                   {
                       Task<IDataResult<PaginatedList<{entityName}ResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);
                       Task<IDataResult<IEnumerable<{entityName}ResponseDto>>> GetAsync();
                       Task<IDataResult<{entityName}ByIdResponseDto>> GetAsync(Guid id);
                       Task<IResult> AddAsync({entityName}CreateRequestDto dto);
                       Task<IResult> UpdateAsync(Guid id, {entityName}UpdateRequestDto dto);
                       Task<IResult> SoftDeleteAsync(Guid id);
                   }
                   """;

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }
}