using StorageBot.Domain.Abstract;
using StorageBot.Domain.Account;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Storing
{
    public class Storage : BaseStorageModel
    {
        private static void CheckItemForException<T>(List<T> values, T value, bool itemExpectation, string valueName)
        {
            bool has = values.Contains(value);
            if (has == itemExpectation)
            {
                string text = itemExpectation ? $"Storage already contains this {valueName}"
                                              : $"Storage does not contains this {valueName}";
                throw new ArgumentException(text);
            }
        }

        public UserOwnership UserOwnership { get; protected set; }
        public IReadOnlyCollection<Container> Containers => _containers.AsReadOnly();
        public IReadOnlyCollection<EntryCategory> EntryCategories => _categories.AsReadOnly();
        public IReadOnlyCollection<EntryQuantityType> EntryQuantityTypes => _quantityTypes.AsReadOnly();

        private int _userOwnershipId;
        private List<Container> _containers;
        private List<EntryCategory> _categories;
        private List<EntryQuantityType> _quantityTypes;

        public Storage(UserOwnership userOwnership)
        {
            UserOwnership = userOwnership ?? throw new ArgumentNullException(nameof(userOwnership));
            _userOwnershipId = userOwnership.Id;
            _containers = new List<Container>();
            _categories = new List<EntryCategory>();
            _quantityTypes = new List<EntryQuantityType>();
        }

        protected Storage()
        {
            _containers = new List<Container>();
            _categories = new List<EntryCategory>();
            _quantityTypes = new List<EntryQuantityType>();
            UserOwnership = null!;
        }

        public void AddContainer(Container container)
        {
            CheckItemForException(_containers, container, true, "container");
            _containers.Add(container);
        }

        public void RemoveContainer(Container container)
        {
            CheckItemForException(_containers, container, false, "container");
            _containers.Remove(container);
        }

        public void AddEntryCategory(EntryCategory category)
        {
            CheckItemForException(_categories, category, true, "category");
            _categories.Add(category);
        }

        public void RemoveEntryCategory(EntryCategory category)
        {
            CheckItemForException(_categories, category, false, "category");
            _categories.Remove(category);
        }

        public void AddEntryQuantityType(EntryQuantityType quantityType)
        {
            CheckItemForException(_quantityTypes, quantityType, true, "quantity type");
            _quantityTypes.Add(quantityType);
        }

        public void RemoveEntryQuantityType(EntryQuantityType quantityType)
        {
            CheckItemForException(_quantityTypes, quantityType, false, "quantity type");
            _quantityTypes.Remove(quantityType);
        }
    }
}
