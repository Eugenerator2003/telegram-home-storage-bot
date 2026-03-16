using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Exceptions.Repositories
{
    public class EntityNotFoundException : StorageException
    {
        public Type EntityType { get; }

        public EntityNotFoundException(Type entityType)
        {
            EntityType = entityType;
        }
    }
}
