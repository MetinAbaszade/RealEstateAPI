﻿using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface IPermissionRepository : IGenericRepository<Permission>
{
}