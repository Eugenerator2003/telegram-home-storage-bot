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

            parent.AttachChild(this);
            _parentId = parent.Id;
        }

        public bool IsAncestor(TreeNodeModel<T> parent)
        {
            if (this.parent == null)
                return false;
            if (this.parent.Equals(parent))
                return true;
            else
                return this.parent.IsAncestor(parent);
        }

        public bool IsDescendant(TreeNodeModel<T> descendant)
        {
            if (children.Count == 0)
                return false;
            if (children.Contains(descendant))
                return true;
            else
                return children.Any(c => c.IsDescendant(descendant));
        }

        protected void AddChild(TreeNodeModel<T> child)
        {
            AttachChild(child);
        }

        protected void RemoveChild(TreeNodeModel<T> child)
        {
            child.DetachFromParent();
        }

        protected void AttachChild(TreeNodeModel<T> child)
        {
            if (IsAncestor(child))
                throw new InvalidOperationException("Cannot attach ancestor node as a child");
            if (IsDescendant(child))
                throw new InvalidOperationException("Сannot attach node that is already a descendant");
            if (child.parent != null)
            {
                child.DetachFromParent();
            }
            child.parent = this;
            child._parentId = Id;
            children.Add(child);
        }

        protected void DetachFromParent()
        {
            if (parent == null)
                throw new InvalidOperationException("Cannot detach node without a parent");
            parent.children.Remove(this);
            parent = null;
            _parentId = null;
        }

        protected T? GetParent()
        {
            if (parent == null)
                return null;
            return (T)parent;
        }

        protected IReadOnlyCollection<T> GetChildren()
        {
            return children.Select(c => (T)c).ToList();
        }
    }
}
