using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StorageBot.Domain.Repositories
{
    public class StorageContainerRepository : StorageRepository<Container>
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

        public StorageContainerRepository(Storage storage) 
            : base(storage)
        {

        }

        public override void Create(Container entity)
        {
            storage.AddContainer(entity);
        }

        public override void Delete(int id)
        {
            var container = GetById(id) ??
                throw new EntityNotFoundException(typeof(Container));
            storage.RemoveContainer(container);
        }

        public override IEnumerable<Container> GetAll()
        {
            return storage.Containers;
        }

        public override Container? GetById(int id)
        {
            return GetInner(storage.Containers, id);
        }
    }
}
