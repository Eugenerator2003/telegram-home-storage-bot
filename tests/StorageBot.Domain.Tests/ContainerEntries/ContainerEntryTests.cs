using Moq;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.ContainerEntries
{
    [TestClass]
    public class ContainerEntryTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            string name = "Entry1";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);

            Assert.AreEqual(name, containerEntry.Name);
            Assert.AreEqual(quantity, containerEntry.EntryQuantity);
            Assert.IsEmpty(containerEntry.Categories);
        }

        [TestMethod]
        public void InitializeEmptyNameTest()
        {
            string name = "";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var containerEntry = new ContainerEntry(name, quantity);
            });
            Assert.Contains(nameof(name), exception.Message);
        }

        [TestMethod]
        public void InitializeNullNameTest()
        {
            string? name = null!;
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var containerEntry = new ContainerEntry(name, quantity);
            });
            Assert.Contains(nameof(name), exception.Message);
        }

        [TestMethod]
        public void InitializeNullQuantityTest()
        {
            string? name = null!;
            EntryQuantity quantity = null!;
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var containerEntry = new ContainerEntry(name, quantity);
            });
            Assert.Contains(nameof(name), exception.Message);
        }

        [TestMethod]
        public void AddCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            containerEntry.AddCategory(category);
            Assert.HasCount(1, containerEntry.Categories);
            Assert.AreEqual(category, containerEntry.Categories.ElementAt(0));
        }

        [TestMethod]
        public void AddAssignedCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<EntryAlreadyAssignedToCategoryException>(() =>
            {
                containerEntry.AddCategory(category);
            });
        }

        [TestMethod]
        public void AddDescendantCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            var descendantCategory = new EntryCategory("descendantCategorys", category);
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<AssignedCategoryHierarchyException>(() =>
            {
                containerEntry.AddCategory(descendantCategory);
            });
        }

        [TestMethod]
        public void AddAncestorCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var ancestorCategory = new EntryCategory("ancestorCategory");
            var category = new EntryCategory("category", ancestorCategory);
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<AssignedCategoryHierarchyException>(() =>
            {
                containerEntry.AddCategory(ancestorCategory);
            });
        }

        [TestMethod]
        public void AddRemoveCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            containerEntry.AddCategory(category);
            Assert.HasCount(1, containerEntry.Categories);
            Assert.AreEqual(category, containerEntry.Categories.ElementAt(0));
            containerEntry.RemoveCategory(category);
            Assert.IsEmpty(containerEntry.Categories);
        }

        [TestMethod]
        public void RemoveNonAssignedCategoryTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            var exception = Assert.ThrowsExactly<EntryNotAssignedToCategoryException>(() =>
            {
                containerEntry.RemoveCategory(category);
            });
        }

        [TestMethod]
        public void AddCategoryNullTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                containerEntry.AddCategory(null!);
            });
        }

        [TestMethod]
        public void RemoveCategoryNullTest()
        {
            string name = "Entry";
            var quantity = new EntryQuantity(new EntryQuantityType(isInteger: true, "unit1"));
            var containerEntry = new ContainerEntry(name, quantity);
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                containerEntry.RemoveCategory(null!);
            });
        }
    }
}
