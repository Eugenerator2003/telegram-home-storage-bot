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
    public class ContainerEntryRepository : IRepository<ContainerEntry>
    {
        private Container container;

        public ContainerEntryRepository(Container container)
        {
            this.container = container;
        }

        public void Create(ContainerEntry entity)
        {
            container.AddEntry(entity);
        }

        public void Delete(int id)
        {
            var entry = GetById(id) ??
                throw new InvalidOperationException("Entry does not belong to the specified container");
            container.RemoveEntry(entry);
        }

        public IEnumerable<ContainerEntry> GetAll()
        {
            return container.Entries;
        }

        public ContainerEntry? GetById(int id)
        {
            return container.Entries.FirstOrDefault(x => x.Id == id);
        }

        public void Update(ContainerEntry entity)
        {
            var entry = GetById(entity.Id) ??
                throw new InvalidOperationException("Entry does not belong to the specified container");
            entry.Name = entity.Name;
        }
    }
}
