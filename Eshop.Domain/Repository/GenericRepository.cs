using Eshop.Domain.Context;
using Eshop.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eshop.Domain.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _dbSet;
        #region Ctor
        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        #endregion
        public void EditEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public void EditEntityByUser(TEntity entity, string username)
        {
            entity.LastUpdateDate= DateTime.Now;
            entity.UserName= username;
            _dbSet.Update(entity);
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == entityId);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();    
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
           await _dbSet.AddAsync(entity);

        }
        public async void AddEntityByUser(TEntity entity, string username)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            entity.UserName = username;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeEntities(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await AddEntity(entity);
            }
        }

       public void DeleteEntity(TEntity entity)
        {
            entity.IsDelete = true;
            EditEntity(entity);
        }

        public async Task DeleteEntityById(long entityId)
        {
          TEntity entity= await GetEntityById(entityId);
            if (entity!=null)
            {
                DeleteEntity(entity);
            }
        }

        public void DeletPermanent(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeletPermanentEntities(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void DeletPhisically(TEntity entitiy)
        {
            _dbSet.Remove(entitiy);
        }




        #region Dispose
        public async ValueTask DisposeAsync()
        {
            //todo
        }

        

        #endregion

    }
}
