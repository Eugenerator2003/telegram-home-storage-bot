using StorageBot.Domain.Abstract;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Repositories
{
    public abstract class StorageRepository<T>
        : IDomainRepository<T>
        where T : BaseStorageModel
    {
        protected readonly Storage storage;

        protected StorageRepository(Storage storage)
        {
            this.storage = storage;
        }

        public abstract void Create(T entity);
        public abstract void Delete(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract T? GetById(int id);
    }
}
