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
            T[] expectedTs = new T[3];
            for(int i = 0; i < expectedTs.Length; i++)
            {
                expectedTs[i] = GetEntity();
                AddEntityToStorage(storage, expectedTs[i]);
            }
            var ts = repository.GetAll().ToArray();
            Assert.HasCount(expectedTs.Length, ts);
            for(int i = 0; i < expectedTs.Length; i++)
            {
                Assert.AreEqual(expectedTs[i], ts[i]);
            }
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
            StorageRepository<T> repository = GetRepository(storage);
            var exception = Assert.ThrowsExactly<EntityNotFoundException>(() =>
            {
                repository.Delete(9);
            });
        }

        public virtual void GetByIdTest()
        {
            var storage = GetStorage();
            var expectedEntity = GetEntity();
            expectedEntity.Id = 10;
            AddEntityToStorage(storage, expectedEntity);
            var repository = GetRepository(storage);
            var entity = repository.GetById(expectedEntity.Id);
            Assert.AreEqual(expectedEntity, entity);
        }

        public abstract void CreateTest();

        protected Storage GetStorage()
        {
            return new UserOwnership(new User()).PersonalStorage;
        }

        protected abstract T GetEntity();
        protected abstract StorageRepository<T> GetRepository(Storage storage);
        protected abstract void AddEntityToStorage(Storage storage, T entity);
        protected abstract IEnumerable<T> GetStorageTs(Storage storage);
    }
}
