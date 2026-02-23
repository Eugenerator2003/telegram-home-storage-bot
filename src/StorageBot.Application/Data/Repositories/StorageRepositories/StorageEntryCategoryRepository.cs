using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Data.Repositories.StorageRepositories
{
    public class StorageEntryCategoryRepository : IRepository<EntryCategory>
    {
        private static EntryCategory? GetSub(IEnumerable<EntryCategory> categories, int id)
        {
            EntryCategory? sub = null;
            Queue<EntryCategory> queue = new Queue<EntryCategory>(categories);
            while (queue.Count > 0 && sub == null)
            {
                EntryCategory category = queue.Dequeue();
                if (category.Id == id)
                {
                    sub = category;
                }
                else
                {
                    foreach(var s in category.Subcategories)
                    {
                        queue.Enqueue(s);
                    }
                }
            }
            return sub;
        }

        private Storage storage;

        public StorageEntryCategoryRepository(Storage storage)
        {
            this.storage = storage;
        }

        public void Create(EntryCategory entity)
        {
            storage.AddEntryCategory(entity);
        }

        public void Delete(int id)
        {
            var category = GetById(id) ??
                throw new InvalidOperationException("Category does not belong to the specified storage");
            storage.RemoveEntryCategory(category);
        }

        public IEnumerable<EntryCategory> GetAll()
        {
            return storage.EntryCategories;
        }

        public EntryCategory? GetById(int id)
        {
            return GetSub(storage.EntryCategories, id);
        }

        public void Update(EntryCategory entity)
        {
            var category = GetById(entity.Id) ??
                throw new InvalidOperationException("Category does not belong to the specified storage");
            category.Name = entity.Name;
        }
    }
}
