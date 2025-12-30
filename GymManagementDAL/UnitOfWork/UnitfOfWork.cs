using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.UnitOfWork
{
    public class UnitfOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<Type, object> _repostories = new();

        public ISessionRepository SessionRepository { get; }

        public UnitfOfWork(GymDbContext dbContext, ISessionRepository sessionRepository)
         {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);
            if (_repostories.TryGetValue(entityType, out var repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            else
            {
                var newRepo = new GenericRepository<TEntity>(_dbContext);
                _repostories[entityType] = newRepo;
                return newRepo;
            }

        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
