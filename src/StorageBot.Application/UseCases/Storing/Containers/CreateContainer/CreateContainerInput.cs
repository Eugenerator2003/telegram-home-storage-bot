using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.CreateContainer
{
    public class CreateContainerInput : StorageInput
    {
        public int? OuterContainerId { get; }

        public string ContainerName { get; }

        public CreateContainerInput(int userId, int storageOwnershipId, int? outerContainerId, string name) 
            : base(userId, storageOwnershipId)
        {
            OuterContainerId = outerContainerId;
            ContainerName = name;
        }
    }
}
