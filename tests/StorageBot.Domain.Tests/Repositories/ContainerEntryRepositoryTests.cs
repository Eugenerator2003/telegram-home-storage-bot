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
            EntryQuantityType quantityType = new EntryQuantityType(isInteger: true, "quantity");
            for(int i = 0; i < 3; i++)
            {
                ContainerEntry entry = new ContainerEntry($"entry {i}", new EntryQuantity(quantityType));
                container.AddEntry(entry);
            }
            var expectedEntries = container.Entries.ToArray();
            var entries = repository.GetAll().ToArray();
            Assert.HasCount(expectedEntries.Length, entries);
            for(int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expectedEntries[i], entries[i]);
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var entryName = "entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: false, "unit"));
            Container container = new Container("container");
            ContainerEntryRepository repository = new ContainerEntryRepository(container);
            Assert.HasCount(0, container.Entries);
            repository.Create(entryName, quantity);
            Assert.HasCount(1, container.Entries);
            var entry = container.Entries.ElementAt(0);
            Assert.AreEqual(entryName, entry.Name);
            Assert.AreEqual(quantity, entry.EntryQuantity);
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
