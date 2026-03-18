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
    public class StorageEntryQuantityTypeRepository : StorageRepository<EntryQuantityType>
    {
        public StorageEntryQuantityTypeRepository(Storage storage)
            : base(storage)
        {
        }

        public void Create(bool isInteger, string quantityName)
        {
            EntryQuantityType type = new EntryQuantityType(isInteger, quantityName);
            storage.AddEntryQuantityType(type);
        }

        public override void Delete(int id)
        {
            var quantityType = GetById(id) ??
                throw new EntityNotFoundException(typeof(EntryQuantityType));
            storage.RemoveEntryQuantityType(quantityType);
        }

        public override IEnumerable<EntryQuantityType> GetAll()
        {
            return storage.EntryQuantityTypes;
        }

        public override EntryQuantityType? GetById(int id)
        {
            return storage.EntryQuantityTypes.FirstOrDefault(x => x.Id == id);
        }
    }
}
