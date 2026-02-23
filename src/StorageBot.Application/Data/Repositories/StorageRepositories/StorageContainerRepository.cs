using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StorageBot.Application.Data.Repositories.StorageRepositories
{
    public class StorageContainerRepository : IRepository<Container>
    {
        private static Container? GetInner(IEnumerable<Container> containers, int id)
        {
            Container? inner = null;
            Queue<Container> queue = new Queue<Container>(containers);
            while (queue.Count > 0 && inner == null)
            {
                Container container = queue.Dequeue();
                if (container.Id == id)
                {
                    inner = container;
                }
                else
                {
                    foreach (var i in container.InnerContainers)
                    {
                        queue.Enqueue(i);
                    }
                }
            }
            return inner;
        }

        private Storage storage;

        public StorageContainerRepository(Storage storage)
        {
            this.storage = storage;
        }

        public void Create(Container entity)
        {
            storage.AddContainer(entity);
        }

        public void Delete(int id)
        {
            var container = GetById(id) ??
                throw new InvalidOperationException("Container does not belong to the specified storage");
            storage.RemoveContainer(container);
        }

        public IEnumerable<Container> GetAll()
        {
            return storage.Containers;
        }

        public Container? GetById(int id)
        {
            return GetInner(storage.Containers, id);
        }

        public void Update(Container entity)
        {
            var container = GetById(entity.Id) ??
               throw new InvalidOperationException("Container does not belong to the specified storage");
            container.Name = entity.Name;
        }
    }
}
