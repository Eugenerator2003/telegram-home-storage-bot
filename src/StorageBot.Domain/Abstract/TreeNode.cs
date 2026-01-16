using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Abstract
{
    public abstract class TreeNodeModel<T> : BaseStorageModel
        where T : TreeNodeModel<T>
    {
        public string Name { get; set; }

        protected TreeNodeModel<T>? parent;
        protected List<TreeNodeModel<T>> children;

        private int? _parentId;

        public TreeNodeModel(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            children = new List<TreeNodeModel<T>>();
        }

        public TreeNodeModel(string name, TreeNodeModel<T> parent)
            : this(name)
        {
            ArgumentNullException.ThrowIfNull(parent);

            AttachChild(this, parent);
            _parentId = parent.Id;
        }

        protected void AddChild(TreeNodeModel<T> child)
        {
            AttachChild(child, this);
        }

        protected void RemoveChild(TreeNodeModel<T> child)
        {
            DetachChild(child);
        }

        protected void AttachChild(TreeNodeModel<T> child, TreeNodeModel<T> parent)
        {
            if (parent.children.Contains(child))
                throw new InvalidOperationException("Node already have the child");
            DetachChild(child);
            child.parent = parent;
            child._parentId = parent.Id;
            parent.children.Add(child);
        }

        protected void DetachChild(TreeNodeModel<T> child)
        {
            if (child.parent == null)
                throw new InvalidOperationException("Node have no parent");
            child.parent.children.Remove(child);
            child.parent = null;
            child._parentId = 0;
        }

        protected T? GetParent()
        {
            if (parent == null)
                return null;
            return (T)parent;
        }

        protected IEnumerable<T> GetChildrens()
        {
            return children.Select(c => (T)c);
        }
    }
}
