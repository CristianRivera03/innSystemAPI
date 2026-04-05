using InnSystem.DAL.DBConext;
using InnSystem.DAL.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InnSystem.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly InnDbContext _dbcontext;

        public GenericRepository(InnDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                return await _dbcontext.Set<TModel>().FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving record from {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TModel> GetById(object id)
        {
            try
            {
                return await _dbcontext.Set<TModel>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving {typeof(TModel).Name} by ID: {ex.Message}", ex);
            }
        }

        public async Task<bool> Exists(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                return await _dbcontext.Set<TModel>().AnyAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking existence in {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Add(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database update error while creating {typeof(TModel).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> Update(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database update error while updating {typeof(TModel).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> HardDelete(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Remove(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while deleting {typeof(TModel).Name}. It might be in use: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> SoftDelete(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while soft deleting {typeof(TModel).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error soft deleting {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddRange(IEnumerable<TModel> entities)
        {
            try
            {
                _dbcontext.Set<TModel>().AddRange(entities);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while adding range of {typeof(TModel).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding range of {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> RemoveRange(IEnumerable<TModel> entities)
        {
            try
            {
                _dbcontext.Set<TModel>().RemoveRange(entities);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error while removing range of {typeof(TModel).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing range of {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public async Task<(IEnumerable<TModel> Data, int TotalRecords)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                IQueryable<TModel> query = filter == null ? _dbcontext.Set<TModel>() : _dbcontext.Set<TModel>().Where(filter);
                int totalRecords = await query.CountAsync();
                
                var data = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
                                
                return (data, totalRecords);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving paged data for {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }

        public IQueryable<TModel> Query(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                return filter == null ? _dbcontext.Set<TModel>() : _dbcontext.Set<TModel>().Where(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error building query for {typeof(TModel).Name}: {ex.Message}", ex);
            }
        }
    }
}
