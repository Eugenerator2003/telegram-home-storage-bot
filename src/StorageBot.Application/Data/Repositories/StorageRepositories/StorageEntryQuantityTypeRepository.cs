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
    public class StorageEntryQuantityTypeRepository : IRepository<EntryQuantityType>
    {
        private Storage storage;

        public StorageEntryQuantityTypeRepository(Storage storage)
        {
            this.storage = storage;
        }

        public void Create(EntryQuantityType entity)
        {
            storage.AddEntryQuantityType(entity);
        }

        public void Delete(int id)
        {
            var quantityType = GetById(id) ?? 
                throw new InvalidOperationException("Quantity type does not belong to the specified storage");
            storage.RemoveEntryQuantityType(quantityType);
        }

        public IEnumerable<EntryQuantityType> GetAll()
        {
            return storage.EntryQuantityTypes;
        }

        public EntryQuantityType? GetById(int id)
        {
            return storage.EntryQuantityTypes.FirstOrDefault(x => x.Id == id);
        }

        public void Update(EntryQuantityType entity)
        {
            var quantityType = GetById(entity.Id) ??
                throw new InvalidOperationException("Quantity type does not belong to the specified storage");
            quantityType.UnitName = entity.UnitName;
            quantityType.IsInteger = entity.IsInteger;
        }
    }
}
