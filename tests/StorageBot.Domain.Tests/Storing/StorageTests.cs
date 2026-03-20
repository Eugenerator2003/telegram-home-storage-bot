using StorageBot.Domain.Account;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Storing;
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
            Assert.ThrowsExactly<StorageAlreadyContainsContainerException>(() =>
            {
                storage.AddContainer(container);
            });
        }

        [TestMethod]
        public void AddContainerWithOuterContainerTest()
        {
            var storage = new Storage(ownership);
            var outerContainer = new Container("outer-container");
            var innerContainer = new Container("inner-container", outerContainer);
            Assert.ThrowsExactly<StorageContainerHasOuterContainerException>(() =>
            {
                storage.AddContainer(innerContainer);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedContainerTest()
        {
            var storage = new Storage(ownership);
            Assert.HasCount(0, storage.Containers);
            var container = new Container("container");
            Assert.ThrowsExactly<StorageNotContainsContainerException>(() =>
            {
                storage.RemoveContainer(container);
            });
        }

        [TestMethod]
        public void AddContainerNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.AddContainer(null!);
            });
        }

        [TestMethod]
        public void RemoveContainerNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.RemoveContainer(null!);
            });
        }

        [TestMethod]
        public void RemoveContainerWithInnerContainersTest()
        {
            var storage = new Storage(ownership);
            var container1 = new Container("container1");
            var container2 = new Container("container1", container1);
            storage.AddContainer(container1);
            Assert.ThrowsExactly<StorageContainerHasInnerContainerException>(() =>
            {
                storage.RemoveContainer(container1);
            });
        }

        [TestMethod]
        public void RemoveNotEmptyContainerTest()
        {
            var storage = new Storage(ownership);
            var container1 = new Container("container1");
            var quantityType = new EntryQuantityType(isInteger: false, "unit");
            var quantity = new EntryQuantity(quantityType, 9999);
            container1.AddEntry(new ContainerEntry("entry", quantity));
            storage.AddContainer(container1);
            Assert.ThrowsExactly<StorageContainerNotEmptyException>(() =>
            {
                storage.RemoveContainer(container1);
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
            Assert.ThrowsExactly<StorageAlreadyContainsEntryCategoryException>(() =>
            {
                storage.AddEntryCategory(category);
            });
        }

        [TestMethod]
        public void AddCategoryWithMainCategoryTest()
        {
            var storage = new Storage(ownership);
            var mainCategory = new EntryCategory("main-category");
            var subCategory = new EntryCategory("sub-category", mainCategory);
            Assert.ThrowsExactly<StorageEntryCategoryHasMainCategoryException>(() =>
            {
                storage.AddEntryCategory(subCategory);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedCategoryTest()
        {
            var storage = new Storage(ownership);
            var category = new EntryCategory("category");
            Assert.ThrowsExactly<StorageNotContainsEntryCategoryException>(() =>
            {
                storage.RemoveEntryCategory(category);
            });
        }

        [TestMethod]
        public void AddCategoryNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.AddEntryCategory(null!);
            });
        }

        [TestMethod]
        public void RemoveCategoryNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.RemoveContainer(null!);
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
            Assert.ThrowsExactly<StorageAlreadyContainsQuantityTypeException>(() =>
            {
                storage.AddEntryQuantityType(quantityType);
            });
        }

        [TestMethod]
        public void RemoveNotIncludedQuantityTypeTest()
        {
            var storage = new Storage(ownership);
            var quantityType = new EntryQuantityType(true, "quantity-type");
            Assert.ThrowsExactly<StorageNotContainsQuantityTypeException>(() =>
            {
                storage.RemoveEntryQuantityType(quantityType);
            });
        }

        [TestMethod]
        public void AddQuantityTypeNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.AddEntryQuantityType(null!);
            });
        }

        [TestMethod]
        public void RemoveQuantityTypeNullTest()
        {
            var storage = new Storage(ownership);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                storage.RemoveEntryQuantityType(null!);
            });
        }
    }
}
