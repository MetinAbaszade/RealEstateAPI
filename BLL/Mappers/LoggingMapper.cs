﻿using AutoMapper;
using DTO.Logging;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class LoggingMapper : Profile
{
    public LoggingMapper()
    {
        CreateMap<RequestLogDto, RequestLog>();
        CreateMap<ResponseLogDto, ResponseLog>();
    }
}