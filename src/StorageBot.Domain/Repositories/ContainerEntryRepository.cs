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
    public class ContainerEntryRepository : IDomainRepository<ContainerEntry>
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
                throw new EntityNotFoundException(typeof(ContainerEntry));
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
    }
}
