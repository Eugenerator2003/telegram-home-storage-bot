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
    public class StorageEntryQuantityTypeRepositoryTests
        : StorageRepositoryTests<EntryQuantityType>
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

        protected override void AddEntityToStorage(Storage storage, EntryQuantityType entity)
        {
            storage.AddEntryQuantityType(entity);
        }

        protected override EntryQuantityType GetEntity()
        {
            return new EntryQuantityType(isInteger: true, "unit");
        }

        protected override EntryQuantityType GetEntity(string name)
        {
            return new EntryQuantityType(isInteger: true, name);
        }

        protected override StorageRepository<EntryQuantityType> GetRepository(Storage storage)
        {
            return new StorageEntryQuantityTypeRepository(storage);
        }

        protected override IEnumerable<EntryQuantityType> GetStorageTs(Storage storage)
        {
            return storage.EntryQuantityTypes;
        }

        protected override void RemoveEntityFromStorage(Storage storage, EntryQuantityType entity)
        {
            storage.RemoveEntryQuantityType(entity);
        }
    }
}
