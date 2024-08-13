using DAL.EntityFramework.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityFramework.Abstract;
internal interface IUnifiedRepository<T> : IGenericRepository<T> where T : class
{
}
