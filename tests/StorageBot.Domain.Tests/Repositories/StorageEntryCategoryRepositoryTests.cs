using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Repositories
{
    [TestClass]
    public class StorageEntryCategoryRepositoryTests :
        StorageRepositoryTests<EntryCategory>
    {
        [TestMethod]
        public override void CreateTest()
        {
            base.CreateTest();
        }

        [TestMethod]
        public override void DeleteExceptionTest()
        {
            base.DeleteExceptionTest();
        }

        [TestMethod]
        public override void DeleteTest()
        {
            base.DeleteTest();
        }

        [TestMethod]
        public override void GetAllTest()
        {
            base.GetAllTest();
        }

        [TestMethod]
        public override void GetByIdNullTest()
        {
            base.GetByIdNullTest();
        }

        [TestMethod]
        public override void GetByIdTest()
        {
            base.GetByIdTest();
        }

        protected override void AddEntityToStorage(Storage storage, EntryCategory entity)
        {
            storage.AddEntryCategory(entity);
        }

        protected override EntryCategory GetEntity()
        {
            return new EntryCategory("category");
        }

        protected override EntryCategory GetEntity(string name)
        {
            return new EntryCategory(name);
        }

        protected override StorageRepository<EntryCategory> GetRepository(Storage storage)
        {
            return new StorageEntryCategoryRepository(storage);
        }

        protected override IEnumerable<EntryCategory> GetStorageTs(Storage storage)
        {
            return storage.EntryCategories;
        }

        protected override void RemoveEntityFromStorage(Storage storage, EntryCategory entity)
        {
            storage.RemoveEntryCategory(entity);
        }
    }
}
