using Store.Data.Contexts;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }
        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            // if null then it is the first time to use repo
            if(_repositories is null)
                _repositories = new Hashtable();

            // Key => EntityName 
            var entityKey = typeof(TEntity).Name; // "Product" as string
            // check if the Hash table has a key with this Name
            if (!_repositories.ContainsKey(entityKey))
            {
                // the type of the repository
                var repositoryType = typeof(GenericRepository<,>);
                //create an Instace from this genericRepository
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity),typeof(TKey)),_context);
                // add a new repository 
                _repositories.Add(entityKey, repositoryInstance);
            }
            return (IGenericRepository<TEntity, TKey>)_repositories[entityKey];

        }
    }
}
