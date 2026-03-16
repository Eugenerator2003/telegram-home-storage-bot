using StorageBot.Domain.Abstract;
using StorageBot.Domain.Account;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Repositories
{
    public abstract class StorageRepositoryTests<T>
        where T : BaseStorageModel
    {
        private class User : Domain.Account.User
        {

        }

        public virtual void GetByIdTest()
        {
            Storage storage = GetStorage();
            T expectedEntity = GetEntity();
            AddEntityToStorage(storage, expectedEntity);
            StorageRepository<T> repository = GetRepository(storage);
            var entity = repository.GetById(0);
            Assert.AreEqual(expectedEntity, entity);
        }

        public virtual void GetByIdNullTest()
        {
            Storage storage = GetStorage();
            StorageRepository<T> repository = GetRepository(storage);
            var container = repository.GetById(0);
            Assert.IsNull(container);
        }

        public virtual void GetAllTest()
        {
            Storage storage = GetStorage();
            StorageRepository<T> repository = GetRepository(storage);
            var expectedTs = new T[3];
            for (int i = 0; i < expectedTs.Length; i++)
            {
                var t = GetEntity(i.ToString());
                expectedTs[i] = t;
                repository.Create(t);
            }
            var entries = repository.GetAll();
            for (int i = 0; i < expectedTs.Length; i++)
            {
                Assert.Contains(expectedTs[i], entries);
                Assert.Contains(expectedTs[i], GetStorageTs(storage));
            }
        }

        public virtual void CreateTest()
        {
            Storage storage = GetStorage();
            StorageRepository<T> repository = GetRepository(storage);
            T t = GetEntity();
            Assert.HasCount(0, GetStorageTs(storage));
            repository.Create(t);
            Assert.HasCount(1, GetStorageTs(storage));
            Assert.AreEqual(t, GetStorageTs(storage).ElementAt(0));
        }

        public virtual void DeleteTest()
        {
            Storage storage = GetStorage();
            var t = GetEntity();
            AddEntityToStorage(storage, t);
            StorageRepository<T> repository = GetRepository(storage);
            Assert.HasCount(1, GetStorageTs(storage));
            repository.Delete(t.Id);
            Assert.HasCount(0, GetStorageTs(storage));
        }

        public virtual void DeleteExceptionTest()
        {
            Storage storage = GetStorage();
            T t = GetEntity();
            StorageRepository<T> repository = GetRepository(storage);
            var exception = Assert.ThrowsExactly<EntityNotFoundException>(() =>
            {
                repository.Delete(t.Id);
            });
        }

        protected Storage GetStorage()
        {
            return new UserOwnership(new User()).PersonalStorage;
        }

        protected abstract T GetEntity();
        protected abstract T GetEntity(string name);
        protected abstract StorageRepository<T> GetRepository(Storage storage);
        protected abstract void AddEntityToStorage(Storage storage, T entity);
        protected abstract void RemoveEntityFromStorage(Storage storage, T entity);
        protected abstract IEnumerable<T> GetStorageTs(Storage storage);
    }
}
