using StorageBot.Domain.Account;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Storing
{
    [TestClass]
    public class StorageTests
    {
        private class TestUser() : User
        {

        }

        private UserOwnership ownership = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            User user = new TestUser();
            ownership = new UserOwnership(user);
        }

        [TestMethod]
        public void InitializeStorageTest()
        {
            var storage = new Storage(ownership);
            Assert.AreEqual(ownership, storage.UserOwnership);
            Assert.HasCount(0, storage.Containers);
            Assert.HasCount(0, storage.EntryCategories);
            Assert.HasCount(0, storage.EntryQuantityTypes);
        }

        [TestMethod]
        public void InitializeNullOwnershipTest()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var storage = new Storage(null!);
            });
        }

        [TestMethod]
        public void AddContainerTest()
        {
            var storage = new Storage(ownership);
            var container = new Container("container");
            storage.AddContainer(container);
            Assert.HasCount(1, storage.Containers);
            Assert.AreEqual(container, storage.Containers.ElementAt(0));
        }

        [TestMethod]
        public void AddRemoveContainerTest()
        {
            var storage = new Storage(ownership);
            var container = new Container("container");
            storage.AddContainer(container);
            Assert.HasCount(1, storage.Containers);
            Assert.AreEqual(container, storage.Containers.ElementAt(0));
            storage.RemoveContainer(container);
            Assert.HasCount(0, storage.Containers);
        }

        [TestMethod]
        public void AddAlreadyIncludedContainerTest()
        {
            var storage = new Storage(ownership);
            var container = new Container("container");
            storage.AddContainer(container);
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.AddContainer(container);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedContainerTest()
        {
            var storage = new Storage(ownership);
            Assert.HasCount(0, storage.Containers);
            var container = new Container("container");
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.RemoveContainer(container);
            });
        }

        [TestMethod]
        public void AddCategoryTest()
        {
            var storage = new Storage(ownership);
            var category = new EntryCategory("category");
            storage.AddEntryCategory(category);
            Assert.HasCount(1, storage.EntryCategories);
            Assert.AreEqual(category, storage.EntryCategories.ElementAt(0));
        }

        [TestMethod]
        public void AddRemoveCategoryTest()
        {
            var storage = new Storage(ownership);
            var category = new EntryCategory("category");
            storage.AddEntryCategory(category);
            Assert.HasCount(1, storage.EntryCategories);
            Assert.AreEqual(category, storage.EntryCategories.ElementAt(0));
            storage.RemoveEntryCategory(category);
            Assert.HasCount(0, storage.EntryCategories);
        }

        [TestMethod]
        public void AddAlreadyIncludedCategoryTest()
        {
            var storage = new Storage(ownership);
            var category = new EntryCategory("category");
            storage.AddEntryCategory(category);
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.AddEntryCategory(category);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedCategoryTest()
        {
            var storage = new Storage(ownership);
            var category = new EntryCategory("category");
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.RemoveEntryCategory(category);
            });
        }

        [TestMethod]
        public void AddQuantityTypeTest()
        {
            var storage = new Storage(ownership);
            var quantityType = new EntryQuantityType(true, "quantity-type");
            storage.AddEntryQuantityType(quantityType);
            Assert.HasCount(1, storage.EntryQuantityTypes);
            Assert.AreEqual(quantityType, storage.EntryQuantityTypes.ElementAt(0));
        }

        [TestMethod]
        public void AddRemoveQuantityTypeTest()
        {
            var storage = new Storage(ownership);
            var quantityType = new EntryQuantityType(true, "quantity-type");
            storage.AddEntryQuantityType(quantityType);
            Assert.HasCount(1, storage.EntryQuantityTypes);
            Assert.AreEqual(quantityType, storage.EntryQuantityTypes.ElementAt(0));
            storage.RemoveEntryQuantityType(quantityType);
            Assert.HasCount(0, storage.EntryQuantityTypes);
        }

        [TestMethod]
        public void AddAlreadyIncludedQuantityTypeTest()
        {
            var storage = new Storage(ownership);
            var quantityType = new EntryQuantityType(true, "quantity-type");
            storage.AddEntryQuantityType(quantityType);
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.AddEntryQuantityType(quantityType);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedQuantityTypeTest()
        {
            var storage = new Storage(ownership);
            var quantityType = new EntryQuantityType(true, "quantity-type");
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                storage.RemoveEntryQuantityType(quantityType);
            });
        }
    }
}
