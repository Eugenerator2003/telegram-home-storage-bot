using StorageBot.Domain.Abstract;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Account
{
    public class UserOwnership : BaseStorageModel
    {
        public User User { get; protected set; }
        public Storage PersonalStorage { get; protected set; }
        public IReadOnlyCollection<Storage> OtherStorages => _otherStorages.AsReadOnly();

        private int _userId;
        private int _personalStorageId;
        private List<Storage> _otherStorages;

        public UserOwnership(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            PersonalStorage = new Storage(this);
            _otherStorages = new List<Storage>();

            _userId = user.Id;
            _personalStorageId = PersonalStorage.Id;
        }

        protected UserOwnership(User user, IEnumerable<Storage> storages)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            PersonalStorage = new Storage(this);
            _otherStorages = storages.ToList();

            _userId = user.Id;
            _personalStorageId = PersonalStorage.Id;
        }

        protected UserOwnership()
        {
            User = null!;
            PersonalStorage = null!;
            _otherStorages = new List<Storage>();
        }

        public void AddOtherStorage(Storage storage)
        {
            if (PersonalStorage.Equals(storage))
                throw new ArgumentException("Cannot add personal storage as a shared storage", nameof(storage));

            if (_otherStorages.Contains(storage))
                throw new ArgumentException("User already has access to this storage", nameof(storage));

            _otherStorages.Add(storage);
        }

        public void RemoveOtherStorage(Storage storage)
        {
            if (PersonalStorage.Equals(storage))
                throw new ArgumentException("Cannot remove personal storage from other storages list", nameof(storage));

            if (!_otherStorages.Contains(storage))
                throw new ArgumentException("Storage not found in shared list", nameof(storage));

            _otherStorages.Remove(storage);
        }

        public override string ToString()
        {
            return $"Ownership of user with guid: {User.Id}";
        }
    }
}
