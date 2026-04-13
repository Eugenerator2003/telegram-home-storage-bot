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
        public StorageContainerRepository(Storage storage) 
            : base(storage)
        {

        }

        public void Create(string containerName)
        {
            Container container = new Container(containerName);
            storage.AddContainer(container);
        }

        public void CreateWithOuterContainer(string containerName, int outerContainerId)
        {
            Container outerContainer = GetById(outerContainerId) ??
                throw new EntityNotFoundException(typeof(Container));
            Container container = new Container(containerName, outerContainer);
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
            return storage.FindContainer(id);
        }
    }
}
