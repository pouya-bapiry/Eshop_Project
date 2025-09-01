using Eshop.Domain.Dtos.Account;
using Eshop.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Domain.Repository
{
    public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQuery();
        Task AddEntity(TEntity entity);
        Task AddRangeEntities(List<TEntity> entities);
        Task<TEntity> GetEntityById(long entityId);
        void EditEntity(TEntity entity);
        void EditEntityByUser(TEntity entity,string username);
        void AddEntityByUser(TEntity entity,string username);
        void DeleteEntity(TEntity entity);
        Task DeleteEntityById(long entityId);
        void DeletPermanent(TEntity entity);
        void DeletPermanentEntities(List<TEntity> entities);
        void DeletPhisically(TEntity entitiy);
        Task SaveChanges();
        
    }

}
