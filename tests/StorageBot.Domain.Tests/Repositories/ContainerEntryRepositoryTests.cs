using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Repositories
{
    [TestClass]
    public class ContainerEntryRepositoryTests
    {
        [TestMethod]
        public void GetByIdTest()
        {
            Container container = new Container("container");
            var expectedEntry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: true, "quantity")));
            container.AddEntry(expectedEntry);
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            var entry = repository.GetById(0);
            Assert.AreEqual(expectedEntry, entry);
        }

        [TestMethod]
        public void GetByIdNullTest()
        {
            Container container = new Container("container");
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            var entry = repository.GetById(10000);
            Assert.IsNull(entry);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Container container = new Container("container");
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            var expectedEntries = new ContainerEntry[3];
            EntryQuantityType quantityType = new EntryQuantityType(isInteger: true, "quantity");
            for(int i = 0; i < expectedEntries.Length; i++)
            {
                ContainerEntry entry = new ContainerEntry($"entry {i}", new EntryQuantity(quantityType));
                expectedEntries[i] = entry;
                repository.Create(entry);
            }
            var entries = repository.GetAll();
            for(int i = 0; i < expectedEntries.Length; i++)
            {
                Assert.Contains(expectedEntries[i], entries);
                Assert.Contains(expectedEntries[i], container.Entries);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            Container container = new Container("container");
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            Assert.HasCount(0, container.Entries);
            repository.Create(entry);
            Assert.HasCount(1, container.Entries);
            Assert.AreEqual(entry, container.Entries.ElementAt(0));
        }

        [TestMethod]
        public void DeleteTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            container.AddEntry(entry);
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            Assert.HasCount(1, container.Entries);
            repository.Delete(container.Id);
            Assert.HasCount(0, container.Entries);
        }

        [TestMethod]
        public void DeleteExceptionTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            var exception = Assert.ThrowsExactly<EntityNotFoundException>(() =>
            {
                repository.Delete(container.Id);
            });
        }
    }
}
