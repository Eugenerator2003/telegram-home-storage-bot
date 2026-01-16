using StorageBot.Domain.Abstract;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Storing
{
    public class Storage : BaseStorageModel
    {
        public UserOwnership UserOwnership { get; protected set; }
        public IReadOnlyCollection<Container> Containers => _containers.AsReadOnly();

        private int _userOwnershipId;
        private List<Container> _containers;

        public Storage(UserOwnership userOwnership)
        {
            UserOwnership = userOwnership ?? throw new ArgumentNullException(nameof(userOwnership));
            _userOwnershipId = userOwnership.Id;
            _containers = new List<Container>();
        }

        protected Storage()
        {
            _containers = new List<Container>();
            UserOwnership = null!;
        }

        public void AddContainer(Container container)
        {
            if (_containers.Contains(container))
                throw new ArgumentException("Storage already contains this container", nameof(container));

            _containers.Add(container);
        }

        public void RemoveContainer(Container container)
        {
            if (!_containers.Contains(container))
                throw new ArgumentException("Storage does not contain this container", nameof(container));

            _containers.Remove(container);
        }
    }
}
