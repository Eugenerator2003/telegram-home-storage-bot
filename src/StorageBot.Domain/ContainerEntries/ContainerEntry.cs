using StorageBot.Domain.Abstract;
using StorageBot.Domain.Exceptions.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public class ContainerEntry : BaseStorageModel
    {
        public EntryQuantity EntryQuantity { get; protected set; }
        public string Name { get; set; }
        public IReadOnlyCollection<EntryCategory> Categories => _categories.AsReadOnly();

        private List<EntryCategory> _categories;

        public ContainerEntry(string name, EntryQuantity quantity)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Entry name cannot be empty or null");
            }

            Name = name;
            EntryQuantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
            _categories = new List<EntryCategory>();
        }

        protected ContainerEntry()
        {
            Name = string.Empty;
            EntryQuantity = null!;
            _categories = new List<EntryCategory>();
        }

        public void AddCategory(EntryCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            if (_categories.Contains(category))
                throw new EntryAlreadyAssignedToCategoryException();
            if (_categories.Any(c => c.IsDescendant(category)) ||
                _categories.Any(c => c.IsAncestor(category)))
                throw new AssignedCategoryHierarchyException();
            _categories.Add(category);
        }

        public void RemoveCategory(EntryCategory category)
        {
            ArgumentNullException.ThrowIfNull(category);
            if (!_categories.Contains(category))
                throw new EntryNotAssignedToCategoryException();
            _categories.Remove(category);
        }

        public override string? ToString()
        {
            return $"{Name}: {EntryQuantity}";
        }
    }
}
