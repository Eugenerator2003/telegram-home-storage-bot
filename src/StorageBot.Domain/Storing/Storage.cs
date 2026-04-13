using StorageBot.Domain.Abstract;
using StorageBot.Domain.Account;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions;
using StorageBot.Domain.Exceptions.Storing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Storing
{
    public class Storage : BaseStorageModel
    {
        private static void ThrowIfContains<TItem, TException>(List<TItem> values, TItem value)
            where TException: StorageException, new()
        {
            if (values.Contains(value))
            {
                throw new TException();
            }
        }

        private static void ThrowIfNotContains<TItem, TException>(List<TItem> values, TItem value)
            where TException : StorageException, new()
        {
            if (!values.Contains(value))
            {
                throw new TException();
            }
        }

        private static T? FindNodeById<T>(IEnumerable<T> values, Func<T, IEnumerable<T>> childFunc, int id)
            where T : TreeNodeModel<T>
        {
            T? item = null;
            var queue = new Queue<T>(values);
            while(queue.Count > 0)
            {
                var q = queue.Dequeue();
                if (q.Id == id)
                {
                    item = q; 
                    break;
                }
                else
                {
                    foreach(var child in childFunc(q))
                    {
                        queue.Enqueue(child);
                    }
                }
            }
            return item;
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

        public Container? FindContainer(int id)
        {
            return FindNodeById(_containers, c => c.InnerContainers, id);
        }

        public void AddContainer(Container container)
        {
            ArgumentNullException.ThrowIfNull(container);
            if (container.OuterContainer != null)
            {
                throw new StorageContainerHasOuterContainerException();
            }
            ThrowIfContains<Container, StorageAlreadyContainsContainerException>(_containers, container);
            _containers.Add(container);
        }

        public void RemoveContainer(Container container)
        {
            ArgumentNullException.ThrowIfNull(container);
            ThrowIfNotContains<Container, StorageNotContainsContainerException>(_containers, container);
            if (container.InnerContainers.Count > 0)
            {
                throw new StorageContainerHasInnerContainerException();
            }
            if (container.Entries.Count > 0)
            {
                throw new StorageContainerNotEmptyException();
            }
            _containers.Remove(container);
        }

        public EntryCategory? FindEntryCategory(int id)
        {
            return FindNodeById(_categories, c => c.Subcategories, id);
        }

        public void AddEntryCategory(EntryCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            if (category.MainCategory != null)
            {
                throw new StorageEntryCategoryHasMainCategoryException();
            }
            ThrowIfContains<EntryCategory, StorageAlreadyContainsEntryCategoryException>(_categories, category);
            _categories.Add(category);
        }

        public void RemoveEntryCategory(EntryCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            ThrowIfNotContains<EntryCategory, StorageNotContainsEntryCategoryException>(_categories, category);
            if (category.Subcategories.Count > 0)
            {
                throw new StorageEntryCategoryHasSubcategoriesException();
            }
            _categories.Remove(category);
        }

        public void AddEntryQuantityType(EntryQuantityType quantityType)
        {
            ArgumentNullException.ThrowIfNull(quantityType);
            ThrowIfContains<EntryQuantityType, StorageAlreadyContainsQuantityTypeException>(_quantityTypes, quantityType);
            _quantityTypes.Add(quantityType);
        }

        public void RemoveEntryQuantityType(EntryQuantityType quantityType)
        {
            ArgumentNullException.ThrowIfNull(quantityType);
            ThrowIfNotContains<EntryQuantityType, StorageNotContainsQuantityTypeException>(_quantityTypes, quantityType);
            _quantityTypes.Remove(quantityType);
        }
    }
}
