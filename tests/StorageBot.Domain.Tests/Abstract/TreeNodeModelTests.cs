using StorageBot.Domain.Abstract;
using StorageBot.Domain.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Abstract
{
    [TestClass]
    public class TreeNodeModelTests
    {
        private sealed class TestTreeNodeModel : TreeNodeModel<TestTreeNodeModel>
        {
            public TestTreeNodeModel? Parent => (TestTreeNodeModel)parent!;

            public TestTreeNodeModel(string name) : base(name)
            {
            }

            public TestTreeNodeModel(string name, TreeNodeModel<TestTreeNodeModel> parent) : base(name, parent)
            {
            }
        }

        private class TestTreeNodeModelFactory : ITreeNodeModelFactory<TestTreeNodeModel>
        {
            public override TestTreeNodeModel GetTreeNode()
            {
                return new TestTreeNodeModel("test-tree-node");
            }

            public override TestTreeNodeModel GetTreeNodeWithParent(TestTreeNodeModel parent)
            {
                return new TestTreeNodeModel("test-tree-node", parent);
            }
        }

        private TreeNodeTester<TestTreeNodeModel> tester = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            tester = new TreeNodeTester<TestTreeNodeModel>(new TestTreeNodeModelFactory());
        }

        [TestMethod]
        public void InitializeTest()
        {
            var name = "node";
            var node = new TestTreeNodeModel(name);
            Assert.AreEqual(name, node.Name);
            Assert.IsNull(node.Parent);
        }

        [TestMethod]
        public void InitializeParentTest()
        {
            var parent = new TestTreeNodeModel("parent-node");
            var name = "node";
            var node = new TestTreeNodeModel(name, parent);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(parent, node.Parent);
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
    }
}
