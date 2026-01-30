using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public sealed class EntryCategory : TreeNodeModel<EntryCategory>
    {
        public EntryCategory? MainCategory => GetParent();
        public IReadOnlyCollection<EntryCategory> Subcategories => GetChildren();

        public EntryCategory(string categoryName)
            : base(categoryName)
        {

        }

        public EntryCategory(string categoryName, EntryCategory mainCategory)
            : base(categoryName, mainCategory)
        {

        }

        public void AddSubcategory(EntryCategory subcategory)
        {
            AddChild(subcategory);
        }

        public void RemoveSubcategory(EntryCategory subcategory)
        {
            RemoveChild(subcategory);
        }
    }
}
