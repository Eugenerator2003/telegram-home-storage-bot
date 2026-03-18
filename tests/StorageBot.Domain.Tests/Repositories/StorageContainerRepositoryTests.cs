using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
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
    public class StorageContainerRepositoryTests 
        : StorageRepositoryTests<Container>
    {
        [TestMethod]
        public void CreateWithOuterContainerTest()
        {
            var storage = GetStorage();
            var outerContainer = new Container("outer-container");
            storage.AddContainer(outerContainer);
            var repository = new StorageContainerRepository(storage);
            string innerContainerName = "inner-container";
            repository.CreateWithOuterContainer(innerContainerName, outerContainer.Id);
            var innerContainer = outerContainer.InnerContainers.FirstOrDefault(c => c.Name.Equals(innerContainerName));
            Assert.IsNotNull(innerContainer);
        }

        [TestMethod]
        public void CreateWithOuterContainerExceptionTest()
        {
            var storage = GetStorage();
            var repository = new StorageContainerRepository(storage);
            Assert.ThrowsExactly<EntityNotFoundException>(() => repository.CreateWithOuterContainer("inner-container", 999));
        }

        [TestMethod]
        public void GetByIdDeepTest()
        {
            var container1 = new Container("container1");
            container1.Id = 1;
            var container2 = new Container("container2");
            container2.Id = 2;
            var container3 = new Container("container3");
            container3.Id = 3;
            container1.AddInnerContainer(container2);
            container2.AddInnerContainer(container3);
            var storage = GetStorage();
            storage.AddContainer(container1);
            var repository = new StorageContainerRepository(storage);
            var categoryById = repository.GetById(3);
            Assert.AreEqual(container3, categoryById);
        }

        [TestMethod]
        public override void CreateTest()
        {
            var containerName = "container";
            var storage = GetStorage();
            var repository = new StorageContainerRepository(storage);
            repository.Create(containerName);
            Assert.HasCount(1, storage.Containers);
            var container = storage.Containers.First();
            Assert.AreEqual(containerName, container.Name);
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

        protected override StorageRepository<Container> GetRepository(Storage storage)
        {
            return new StorageContainerRepository(storage);
        }

        protected override IEnumerable<Container> GetStorageTs(Storage storage)
        {
            return storage.Containers;
        }
    }
}
