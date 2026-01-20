using Moq;
using StorageBot.Domain.ContainerEntries;
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
        private class TestEntryQuantity : EntryQuantity
        {
            public TestEntryQuantity()
                : base()
            {

            }

            protected override EntryQuantity AddQuantity(EntryQuantity quantity)
            {
                throw new NotImplementedException();
            }

            protected override void SetValue(string value)
            {
                throw new NotImplementedException();
            }
        }

        private class TestContainerEntry : ContainerEntry
        {
            public TestContainerEntry(string name, EntryQuantity quantity, IEnumerable<EntryCategory> categories)
                : base(name, quantity, categories)
            {

            }
        }

        [TestMethod]
        public void InitializeTest()
        {
            string name = "Entry1";
            var quantity = new TestEntryQuantity();
            var containerEntry = new ContainerEntry(name, quantity);

            Assert.AreEqual(name, containerEntry.Name);
            Assert.AreEqual(quantity, containerEntry.EntryQuantity);
            Assert.IsEmpty(containerEntry.Categories);
        }

        [TestMethod]
        public void InitializeEmptyNameTest()
        {
            string name = "";
            var quantity = new TestEntryQuantity();
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
            var quantity = new TestEntryQuantity();
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
            TestEntryQuantity? quantity = null!;
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
            var quantity = new TestEntryQuantity();
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
            var quantity = new TestEntryQuantity();
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                containerEntry.AddCategory(category);
            });
            Assert.Contains(nameof(category), exception.Message);
        }

        [TestMethod]
        public void AddDescendantCategoryTest()
        {
            string name = "Entry";
            var quantity = new TestEntryQuantity();
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            var descendantCategory = new EntryCategory("descendantCategorys", category);
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                containerEntry.AddCategory(descendantCategory);
            });
            Assert.Contains(nameof(category), exception.Message);
        }

        [TestMethod]
        public void AddAncestorCategoryTest()
        {
            string name = "Entry";
            var quantity = new TestEntryQuantity();
            var containerEntry = new ContainerEntry(name, quantity);
            var ancestorCategory = new EntryCategory("ancestorCategory");
            var category = new EntryCategory("category", ancestorCategory);
            containerEntry.AddCategory(category);
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                containerEntry.AddCategory(ancestorCategory);
            });
            Assert.Contains(nameof(category), exception.Message);
        }

        [TestMethod]
        public void AddRemoveCategoryTest()
        {
            string name = "Entry";
            var quantity = new TestEntryQuantity();
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
            var quantity = new TestEntryQuantity();
            var containerEntry = new ContainerEntry(name, quantity);
            var category = new EntryCategory("category");
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                containerEntry.RemoveCategory(category);
            });
            Assert.Contains(nameof(category), exception.Message);
        }

        [TestMethod]
        public void RemoveCategoriesTest()
        {
            string name = "Entry";
            var quantity = new TestEntryQuantity();
            List<EntryCategory> categories = [new EntryCategory("category1"), new EntryCategory("category2"), new EntryCategory("category3")];
            var containerEntry = new TestContainerEntry(name, quantity, categories);
            int[] indices = [2, 1, 0];
            for(int i = 0; i < 3; i++)
            {
                var index = indices[i];
                var category = categories[index];
                containerEntry.RemoveCategory(category);
                categories.Remove(category);
                Assert.HasCount(3 - i - 1, containerEntry.Categories.ToArray());
            }
        }
    }
}
