using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer
{
    public class GetSingleContainerInput : StorageInput
    {
        public int ContainerId { get; }

        public GetSingleContainerInput(int userId, int storageOwnershipId, int containerId) : base(userId, storageOwnershipId)
        {
            ContainerId = containerId;
        }
    }
}
