using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Repositories
{
    public class StorageEntryCategoryRepository : StorageRepository<EntryCategory>
    {
        public StorageEntryCategoryRepository(Storage storage)
            : base(storage)
        {
        }

        public void CreateWithMainCategory(string categoryName, int mainCategoryId)
        {
            EntryCategory? mainCategory = GetById(mainCategoryId) ??
                throw new EntityNotFoundException(typeof(EntryCategory));
            EntryCategory category = new EntryCategory(categoryName, mainCategory);
        }

        public void Create(string categoryName)
        {
            EntryCategory category = new EntryCategory(categoryName);
            storage.AddEntryCategory(category);
        }

        public override void Delete(int id)
        {
            var category = GetById(id) ??
                throw new EntityNotFoundException(typeof(EntryCategory));
            storage.RemoveEntryCategory(category);
        }

        public override IEnumerable<EntryCategory> GetAll()
        {
            return storage.EntryCategories;
        }

        public override EntryCategory? GetById(int id)
        {
            return storage.FindEntryCategory(id);
        }
    }
}
