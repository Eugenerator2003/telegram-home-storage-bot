using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Repositories
{
    public interface IDomainRepository<T> where T : BaseStorageModel
    {
        T? GetById(int id);

        IEnumerable<T> GetAll();

        void Create(T entity);

        void Delete(int id);
    }
}
