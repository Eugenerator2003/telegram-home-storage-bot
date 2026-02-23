using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Abstract;
using StorageBot.Domain.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.ContainerEntries
{
    [TestClass]
    public class EntryCategoryTests
    {
        private class TestEntryCategoryFactory : ITreeNodeModelFactory<EntryCategory>
        {
            public override EntryCategory GetTreeNode()
            {
                return new EntryCategory("category");
            }

            public override EntryCategory GetTreeNodeWithParent(EntryCategory parent)
            {
                return new EntryCategory("category", parent);
            }
        }

        private TreeNodeTester<EntryCategory> tester = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            tester = new TreeNodeTester<EntryCategory>(new TestEntryCategoryFactory());
        }

        [TestMethod]
        public void InitializeNoParentTest()
        {
            string name = "category";
            var category = new EntryCategory(name);
            Assert.IsNull(category.MainCategory);
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void InitializeParentTest()
        {
            string name = "category";
            var mainCategory = new EntryCategory("mainCategory");
            var category = new EntryCategory(name, mainCategory);
            Assert.IsNotNull(category.MainCategory);
            Assert.AreEqual(mainCategory, category.MainCategory);
            Assert.AreEqual(name, category.Name);
        }

        [TestMethod]
        public void AddSubcategorySuccessTest()
        {
            var category = new EntryCategory("category");
            var subcategory = new EntryCategory("subcategory");
            Assert.HasCount(0, category.Subcategories);
            category.AddSubcategory(subcategory);
            Assert.HasCount(1, category.Subcategories);
            Assert.AreEqual(subcategory, category.Subcategories.ElementAt(0));
        }

        [TestMethod]
        public void AddSubcategoryAncestorFailTest()
        {
            var category = new EntryCategory("category");
            var subcategory = new EntryCategory("subcategory", category);
            var exception = Assert.ThrowsExactly<AncestorNodeAsChildException>(() =>
            {
                subcategory.AddSubcategory(category);
            });
        }

        [TestMethod]
        public void AddSubcategoryDescendantFailTest()
        {
            var category = new EntryCategory("category");
            var subcategory1 = new EntryCategory("subcategory1", category);
            var subcategory2 = new EntryCategory("subcategory2", subcategory1);
            var exception = Assert.ThrowsExactly<AlreadyDescendantNodeException>(() =>
            {
                category.AddSubcategory(subcategory2);
            });
        }

        [TestMethod] 
        public void AddRemoveSubcategoryTest()
        {
            var category = new EntryCategory("category");
            var subcategory = new EntryCategory("subcategory");
            Assert.HasCount(0, category.Subcategories);
            category.AddSubcategory(subcategory);
            Assert.HasCount(1, category.Subcategories);
            Assert.AreEqual(subcategory, category.Subcategories.ElementAt(0));
            category.RemoveSubcategory(subcategory);
            Assert.HasCount(0, subcategory.Subcategories);
        }

        [TestMethod]
        public void RemoveNotIncludedSubcategoryTest()
        {
            var category = new EntryCategory("category");
            var subcategory = new EntryCategory("subcategory");
            Assert.ThrowsExactly<NoTreeNodeParentException>(() =>
            {
                category.RemoveSubcategory(category);
            });
        }

        [TestMethod]
        public void AddSubcategoryNullTest()
        {
            var category = new EntryCategory("category");
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                category.AddSubcategory(null!);
            });
        }

        [TestMethod]
        public void RemoveSubcategoryNullTest()
        {
            var category = new EntryCategory("category");
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                category.RemoveSubcategory(null!);
            });
        }

        [TestMethod]
        public void IsAncestorParentSuccessTest()
        {
            tester.IsAncestorParentSuccessTest();
        }

        [TestMethod]
        public void IsAncestorDeepSuccessTest()
        {
            tester.IsAncestorDeepSuccessTest();
        }

        [TestMethod]
        public void IsAncestorFromParentFailTest()
        {
            tester.IsAncestorFromParentFailTest();
        }

        [TestMethod]
        public void IsAncestorFailTest()
        {
            tester.IsAncestorFailTest();
        }

        [TestMethod]
        public void IsDescendantChildSuccessTest()
        {
            tester.IsDescendantChildSuccessTest();
        }

        [TestMethod]
        public void IsDescendantDeepSuccessTest()
        {
            tester.IsDescendantDeepSuccessTest();
        }

        [TestMethod]
        public void IsDesdendantFromChildFailTest()
        {
            tester.IsDesdendantFromChildFailTest();
        }

        [TestMethod]
        public void IsDesdentantFailTest()
        {
            tester.IsDesdentantFailTest();
        }

        [TestMethod]
        public void IsDescendantNullTest()
        {
            tester.IsDescendantNullTest();
        }

        [TestMethod]
        public void IsAncestorNullTest()
        {
            tester.IsAncestorNullTest();
        }
    }
}
