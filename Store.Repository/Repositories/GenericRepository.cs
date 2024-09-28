using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;
        public GenericRepository(StoreDbContext context) 
        { 
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
            =>await _context.Set<TEntity>().AddAsync(entity);

        public void DeleteAsync(TEntity entity)
            =>  _context.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync()
            => await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

       // public async Task<TEntity> GetByIdAsNoTrackingAsync(TKey? id)
       //=>await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<TEntity> GetByIdAsync(TKey? id)
            =>await  _context.Set<TEntity>().FindAsync(id);
       
        public  void UpdateAsync(TEntity entity)
        =>  _context.Set<TEntity>().Update(entity);


        public async Task<IReadOnlyList<TEntity>> GetWithSpecificationsAllAsync(ISpecification<TEntity>? spec)
            => await ApplySpecfications(spec).ToListAsync();

        public async Task<TEntity> GetWithSpecificationsByIdAsync(ISpecification<TEntity>? spec)
       => await ApplySpecfications(spec).FirstOrDefaultAsync();


        private IQueryable<TEntity> ApplySpecfications(ISpecification<TEntity> specs)
           => SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specs);

        public async Task<int> GetCountWithSpecificationAsync(ISpecification<TEntity>? spec)
            => await ApplySpecfications(spec).CountAsync();
    }
}
