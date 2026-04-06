using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Tests.TestUtilities;
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
            var isInteger = true;
            var unitName = "type";
            var storage = GetStorage();
            var repository = new StorageEntryQuantityTypeRepository(storage);
            repository.Create(isInteger, unitName);
            Assert.HasCount(1, storage.EntryQuantityTypes);
            var type = storage.EntryQuantityTypes.First();
            Assert.AreEqual(isInteger, type.IsInteger);
            Assert.AreEqual(unitName, type.UnitName);
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

        protected override StorageRepository<EntryQuantityType> GetRepository(Storage storage)
        {
            return new StorageEntryQuantityTypeRepository(storage);
        }

        protected override IEnumerable<EntryQuantityType> GetStorageTs(Storage storage)
        {
            return storage.EntryQuantityTypes;
        }
    }
}
