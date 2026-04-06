using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.TestUtilities
{
    internal abstract class ITreeNodeModelFactory<T> where T : TreeNodeModel<T>
    {
        public abstract T GetTreeNode();

        public abstract T GetTreeNodeWithParent(T parent);
    }
}
