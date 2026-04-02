using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries
{
    public class ContainerInput : StorageInput
    {
        public int ContainerId { get; }

        public ContainerInput(int userId, int storageOwnershipId, int containerId) 
            : base(userId, storageOwnershipId)
        {
            ContainerId = containerId;
        }
    }
}
