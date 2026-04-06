using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Abstract;
using StorageBot.Domain.Exceptions.Storing;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Tests.TestUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Storing
{
    [TestClass]
    public class ContainerTests
    {
        private class TestContainerFactory : ITreeNodeModelFactory<Container>
        {
            public override Container GetTreeNode()
            {
                return new Container("container");
            }

            public override Container GetTreeNodeWithParent(Container parent)
            {
                return new Container("container", parent);
            }
        }

        private TreeNodeTester<Container> tester = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            tester = new TreeNodeTester<Container>(new TestContainerFactory());
        }

        [TestMethod]
        public void InitializeNoParentTest()
        {
            string name = "container";
            var container = new Container(name);
            Assert.IsNull(container.OuterContainer);
            Assert.AreEqual(name, container.Name);
        }

        [TestMethod]
        public void InitializeParentTest()
        {
            string name = "container";
            var outerContainer = new Container("outerContainer");
            var container = new Container(name, outerContainer);
            Assert.IsNotNull(container.OuterContainer);
            Assert.AreEqual(outerContainer, container.OuterContainer);
            Assert.AreEqual(name, container.Name);
        }

        [TestMethod]
        public void AddInnerContainerSuccessTest()
        {
            var container = new Container("container");
            var innerContainer = new Container("inner-container");
            Assert.HasCount(0, container.InnerContainers);
            container.AddInnerContainer(innerContainer);
            Assert.HasCount(1, container.InnerContainers);
            Assert.AreEqual(innerContainer, container.InnerContainers.ElementAt(0));
        }

        [TestMethod]
        public void AddInnerContainerAncestorFailTest()
        {
            var container = new Container("container");
            var innerContainer = new Container("inner-container", container);
            var exception = Assert.ThrowsExactly<AncestorNodeAsChildException>(() =>
            {
                innerContainer.AddInnerContainer(container);
            });
        }

        [TestMethod]
        public void AddInnerContainerDescendantFailTest()
        {
            var container = new Container("container");
            var innerContainer1 = new Container("inner-container1", container);
            var innerContainer2 = new Container("inner-container2", innerContainer1);
            var exception = Assert.ThrowsExactly<AlreadyDescendantNodeException>(() =>
            {
                container.AddInnerContainer(innerContainer2);
            });
        }

        [TestMethod]
        public void AddRemoveInnerContainerTest()
        {
            var container = new Container("container");
            var innerContainer = new Container("inner-container");
            Assert.HasCount(0, container.InnerContainers);
            container.AddInnerContainer(innerContainer);
            Assert.HasCount(1, container.InnerContainers);
            Assert.AreEqual(innerContainer, container.InnerContainers.ElementAt(0));
            container.RemoveInnerContainer(innerContainer);
            Assert.HasCount(0, container.InnerContainers);
        }

        [TestMethod]
        public void RemoveNotIncludedContainerTest()
        {
            var container = new Container("container");
            var innerContainer = new Container("inner-container");
            Assert.ThrowsExactly<NoTreeNodeParentException>(() =>
            {
                container.RemoveInnerContainer(innerContainer);
            });
        }

        [TestMethod]
        public void AddInnerContainerNullTest()
        {
            var container = new Container("container");
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                container.AddInnerContainer(null!);
            });
        }

        [TestMethod]
        public void RemoveInnerContainerNullTest()
        {
            var container = new Container("container");
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                container.RemoveInnerContainer(null!);
            });
        }

        [TestMethod]
        public void AddEntryTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            Assert.HasCount(0, container.Entries);
            container.AddEntry(entry);
            Assert.HasCount(1, container.Entries);
            Assert.AreEqual(entry, container.Entries.ElementAt(0));
        }

        [TestMethod]
        public void AddEntryExceptionTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            Assert.ThrowsExactly<ContainerAlreadyContainsEntryException>(() =>
            {
                container.AddEntry(entry);
                container.AddEntry(entry);
            });
        }

        [TestMethod]
        public void RemoveEntryTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            container.AddEntry(entry);
            Assert.HasCount(1, container.Entries);
            container.RemoveEntry(entry);
            Assert.HasCount(0, container.Entries);
        }

        [TestMethod]
        public void RemoveEntryExceptionTest()
        {
            Container container = new Container("container");
            ContainerEntry entry = new ContainerEntry("entry", new EntryQuantity(new EntryQuantityType(isInteger: false, "unit")));
            Assert.ThrowsExactly<ContainerNotContainsEntryException>(() => container.RemoveEntry(entry));
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
