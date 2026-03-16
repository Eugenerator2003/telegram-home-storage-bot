using StorageBot.Domain.Account;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBot.Domain.Tests.Utils;

namespace StorageBot.Domain.Tests.Repositories
{
    [TestClass]
    public class StorageContainerRepositoryTests 
        : StorageRepositoryTests<Container>
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

        protected override void AddEntityToStorage(Storage storage, Container entity)
        {
            storage.AddContainer(entity);
        }

        protected override Container GetEntity()
        {
            return new Container("container");
        }

        protected override Container GetEntity(string name)
        {
            return new Container(name);
        }

        protected override StorageRepository<Container> GetRepository(Storage storage)
        {
            return new StorageContainerRepository(storage);
        }

        protected override IEnumerable<Container> GetStorageTs(Storage storage)
        {
            return storage.Containers;
        }

        protected override void RemoveEntityFromStorage(Storage storage, Container entity)
        {
            storage.RemoveContainer(entity);
        }
    }
}
