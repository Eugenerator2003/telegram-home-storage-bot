using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.DeleteContainer
{
    public class DeleteContainerInput : StorageInput
    {
        public int ContainerId { get; set; }

        public DeleteContainerInput(int userId, int storageOwnershipId, int containerId) 
            : base(userId, storageOwnershipId)
        {
            ContainerId = containerId;
        }
    }
}
