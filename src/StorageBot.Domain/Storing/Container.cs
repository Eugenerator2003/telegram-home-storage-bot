using StorageBot.Domain.Abstract;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Storing
{
    public sealed class Container : TreeNodeModel<Container>
    {
        public Container? OuterContainer => GetParent();
        public IReadOnlyCollection<Container> InnerContainers => GetChildren();
        public IReadOnlyCollection<ContainerEntry> Entries => _entries.AsReadOnly();

        private List<ContainerEntry> _entries;

        public Container(string containerName)
            : base(containerName)
        {
            _entries = new List<ContainerEntry>();
        }

        public Container(string containerName, Container outerContainer) 
            : base(containerName, outerContainer)
        {
            _entries = new List<ContainerEntry>();
        }

        public void AddInnerContainer(Container innerContainer)
        {
            AddChild(innerContainer);
        }

        public void RemoveInnerContainer(Container innerContainer)
        {
            RemoveChild(innerContainer);
        }

        public void AddEntry(ContainerEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (_entries.Contains(entry))
                throw new ContainerAlreadyContainsEntryException();
            _entries.Add(entry);
        }

        public void RemoveEntry(ContainerEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!_entries.Contains(entry))
                throw new ContainerNotContainsEntryException();
            _entries.Remove(entry);
        }
    }
}
