using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Data.Repositories
{
    public abstract class UserRepository : IRepository<User>
    {
        public abstract void Create(User entity);
        public abstract void Delete(int id);
        public abstract IEnumerable<User> GetAll();
        public abstract User? GetById(int id);
        public abstract void Update(User entity);
    }
}
