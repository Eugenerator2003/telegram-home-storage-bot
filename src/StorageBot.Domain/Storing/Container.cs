using StorageBot.Domain.Abstract;
using StorageBot.Domain.ContainerEntries;
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

        public IEnumerable<Container> InnerContainers => GetChildren();

        public List<ContainerEntry> Entries { get; private set; }

        public Container(string containerName)
            : base(containerName)
        {
            Entries = new List<ContainerEntry>();
        }

        public Container(string containerName, Container outerContainer) 
            : base(containerName, outerContainer)
        {
            Entries = new List<ContainerEntry>();
        }

        public void AddInnerContainer(Container innerContainer)
        {
            AddChild(innerContainer);
        }

        public void RemoveInnerContainer(Container innerContainer)
        {
            RemoveChild(innerContainer);
        }
    }
}
