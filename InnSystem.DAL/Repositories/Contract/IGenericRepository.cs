using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace InnSystem.DAL.Repositories.Contract
{
    public interface IGenericRepository<TModel> where TModel : class
    {

        Task<TModel> Get(Expression<Func<TModel, bool>> filter);
        //Esta optimizado para buscar id
        Task<TModel> GetById(object id);
        //Verifica si existe un registro
        Task<bool> Exists(Expression<Func<TModel, bool>> filter);
        Task<TModel> Create(TModel model);
        Task<bool> Update(TModel model);
        //Erasers
        Task<bool> SoftDelete(TModel model);
        Task<bool> HardDelete(TModel model);

        //Paginacion
        Task<(IEnumerable<TModel> Data, int TotalRecords)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TModel, bool>> filter = null);

        Task<bool> RemoveRange(IEnumerable<TModel> entities);
        Task<bool> AddRange(IEnumerable<TModel> entities);
        IQueryable<TModel> Query(Expression<Func<TModel, bool>> filter = null);
    }
}
