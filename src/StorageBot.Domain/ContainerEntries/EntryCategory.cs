using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public sealed class EntryCategory : TreeNodeModel<EntryCategory>
    {
        public EntryCategory? MainCategory => GetParent();
        public IEnumerable<EntryCategory> Subcategories => GetChildrens();

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
