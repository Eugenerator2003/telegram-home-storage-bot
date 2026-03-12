using StorageBot.Domain.ContainerEntries;
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

        public override void Create(EntryQuantityType entity)
        {
            storage.AddEntryQuantityType(entity);
        }

        public override void Delete(int id)
        {
            var quantityType = GetById(id) ?? 
                throw new InvalidOperationException("Quantity type does not belong to the specified storage");
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
