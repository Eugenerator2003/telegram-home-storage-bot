using StorageBot.Domain.Abstract;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Data.Repositories
{
    public interface IRepository<T> where T : BaseStorageModel
    {
        T? GetById(int id);

        IEnumerable<T> GetAll();

        void Create(T entity);

        void Update(T entity);

        void Delete(int id);
    }
}
