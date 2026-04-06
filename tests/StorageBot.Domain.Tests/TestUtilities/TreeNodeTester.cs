using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.TestUtilities
{
    internal class TreeNodeTester<T> where T : TreeNodeModel<T>
    {
        private ITreeNodeModelFactory<T> _factory;

        public TreeNodeTester(ITreeNodeModelFactory<T> factory)
        {
            _factory = factory;
        }

        public void IsAncestorParentSuccessTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNodeWithParent(parent);
            Assert.IsTrue(child.IsAncestor(parent));
        }

        public void IsAncestorDeepSuccessTest()
        {
            var parent = _factory.GetTreeNode();
            var child1 = _factory.GetTreeNodeWithParent(parent);
            var child2 = _factory.GetTreeNodeWithParent(child1);
            var child3 = _factory.GetTreeNodeWithParent(child2);
            Assert.IsTrue(child3.IsAncestor(parent));
        }

        public void IsAncestorFromParentFailTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNodeWithParent(parent);
            Assert.IsFalse(parent.IsAncestor(child));
        }

        public void IsAncestorFailTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNode();
            Assert.IsFalse(child.IsAncestor(parent));
        }

        public void IsAncestorNullTest()
        {
            var child = _factory.GetTreeNode();
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                child.IsAncestor(null!);
            });
        }

        public void IsDescendantChildSuccessTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNodeWithParent(parent);
            Assert.IsTrue(parent.IsDescendant(child));
        }

        public void IsDescendantDeepSuccessTest()
        {
            var parent = _factory.GetTreeNode();
            var child1_1 = _factory.GetTreeNodeWithParent(parent);
            var child1_2 = _factory.GetTreeNodeWithParent(parent);
            var child1_3 = _factory.GetTreeNodeWithParent(parent);

            var child1_1_1 = _factory.GetTreeNodeWithParent(child1_1);
            var child1_1_1_1 = _factory.GetTreeNodeWithParent(child1_1_1);

            var child1_2_1 = _factory.GetTreeNodeWithParent(child1_2);
            var child1_2_2 = _factory.GetTreeNodeWithParent(child1_2);
            var child1_2_2_1 = _factory.GetTreeNodeWithParent(child1_2_2);
            var child1_2_3 = _factory.GetTreeNodeWithParent(child1_2);

            var child1_3_1 = _factory.GetTreeNodeWithParent(child1_3);
            var child1_3_2 = _factory.GetTreeNodeWithParent(child1_3);
            var child1_3_2_1 = _factory.GetTreeNodeWithParent(child1_3_2);

            Assert.IsTrue(parent.IsDescendant(child1_1_1_1));
            Assert.IsTrue(parent.IsDescendant(child1_2_2_1));
            Assert.IsTrue(parent.IsDescendant(child1_3_2_1));
        }

        public void IsDesdendantFromChildFailTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNodeWithParent(parent);
            Assert.IsFalse(child.IsDescendant(parent));
        }

        public void IsDesdentantFailTest()
        {
            var parent = _factory.GetTreeNode();
            var child = _factory.GetTreeNode();
            Assert.IsFalse(parent.IsDescendant(child));
        }

        public void IsDescendantNullTest()
        {
            var parent = _factory.GetTreeNode();
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                parent.IsDescendant(null!);
            });
        }
    }
}
