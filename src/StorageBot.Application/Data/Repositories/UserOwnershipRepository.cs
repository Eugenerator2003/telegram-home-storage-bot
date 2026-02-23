using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Data.Repositories
{
    public abstract class UserOwnershipRepository : IRepository<UserOwnership>
    {
        public abstract void Create(UserOwnership entity);
        public abstract void Delete(int id);
        public abstract IEnumerable<UserOwnership> GetAll();
        public abstract UserOwnership? GetById(int id);
        public abstract void Update(UserOwnership entity);
        public abstract UserOwnership? GetByUser(User user);
    }
}
