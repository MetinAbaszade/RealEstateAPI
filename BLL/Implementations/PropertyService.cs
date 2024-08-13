using AutoMapper;
using CORE.Abstract;
using CORE.Localization;
using CORE.Utility;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Concrete;
using DAL.EntityFramework.GenericRepository;
using DTO.Property;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Implementations;

public class PropertyService(IGenericRepository<Property> propertyRepository) : IPropertyService
{
    public async Task<IDataResult<List<GetPropertiesResponseDto>>> GetAsPaginatedListAsync(GetPropertiesRequestDto dto)
    {
        var sql = @"
          SELECT
              p.id_property AS Id, 
              p.fk_id_source AS SourceId,
              p.fk_id_link AS LinkId,
              p.Code,
              pt.property_type_name AS PropertyTypeName, 
              ot.operation_type_name AS OperationTypeName, 
              owt.owner_type_name AS OwnerTypeName, 
              p.fk_id_city AS CityId,
              p.address AS Address, 
              d.document_name AS DocumentName,
              p.Price, 
              c.currency AS Currency, 
              p.data AS Data, 
              p.area AS Area, 
              p.general_area AS GeneralArea, 
              p.floor AS Floor, 
              p.floor_of AS FloorOf, 
              r.room_count_name AS RoomCountName, 
              bt.building_type_name AS BuildingTypeName,
              p.cp_name AS ContactPersonName,
              p.cp_phone_number_01 AS ContactPersonPhoneNumber,
              p.cp_phone_number_02 AS ContactPersonSecondPhoneNumber,
              p.cp_phone_number_03 AS ContactPersonThirdPhoneNumber,
              p.images AS Images,
              p.insert_date AS InsertDate,
              p.fk_id_metro AS MetroId,
              rp.repair_rate_name AS RepairRateName,
              t.target_name AS TargetName
         FROM Property p
         LEFT JOIN property_type pt ON p.fk_id_property_type = pt.id_property_type
         LEFT JOIN operation_type ot ON p.fk_id_operation_type = ot.id_operation_type
         LEFT JOIN owner_type owt ON p.fk_id_owner_type = owt.id_owner_type
         LEFT JOIN Document d ON p.fk_id_document = d.id_document
         LEFT JOIN Currency c ON p.fk_id_currency = c.id_currency
         LEFT JOIN room_count r ON p.fk_id_room = r.id_room_count
         LEFT JOIN building_type bt ON p.fk_id_building_type = bt.id_building_type
         LEFT JOIN repair_rate rp ON p.fk_id_repair = rp.id_repair_rate
         LEFT JOIN Target t ON p.fk_id_target = t.id_target
         WHERE 1=1";

        if (dto.BuildingTypeId.HasValue)
        {
            sql += $" AND p.fk_id_building_type = {dto.BuildingTypeId}";
        }
        if (dto.DocumentId.HasValue)
        {
            sql += $" AND p.fk_id_document = {dto.DocumentId}";
        }
        if (dto.OperationTypeId.HasValue)
        {
            sql += $" AND p.fk_id_operation_type = {dto.OperationTypeId}";
        }
        if (dto.OwnerTypeId.HasValue)
        {
            sql += $" AND p.fk_id_owner_type = {dto.OwnerTypeId}";
        }
        if (dto.PropertyTypeId.HasValue)
        {
            sql += $" AND p.fk_id_property_type = {dto.PropertyTypeId}";
        }
        //if (dto.RegionId.HasValue)
        //{
        //    sql += $" AND p.FkIdCity = {dto.RegionId}";
        //}
        if (dto.RoomCountId.HasValue)
        {
            sql += $" AND p.fk_id_room = {dto.RoomCountId}";
        }

        sql += $" ORDER BY p.id_property OFFSET {(dto.PageIndex - 1) * dto.PageSize} ROWS FETCH NEXT {dto.PageSize} ROWS ONLY";

        var properties = await propertyRepository.ExecuteRawSqlAsync<GetPropertiesResponseDto>(sql);
        return new SuccessDataResult<List<GetPropertiesResponseDto>>(properties, EMessages.Success.Translate());
    }

    public async Task<IDataResult<Property>> GetAsync(int id)
    {
        var property = await propertyRepository.SingleOrDefaultAsync(p => p.IdProperty == id);
        if (property == null)
            return new ErrorDataResult<Property>(EMessages.DataNotFound.Translate());
        return new SuccessDataResult<Property>(property, EMessages.Success.Translate());
    }
}
