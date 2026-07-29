using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer
{
    public class GetOuterContainerInput : StorageInput
    {
        public int InnerContainerId { get; }

        public GetOuterContainerInput(int userId, int storageOwnershipId, int innerContainerId)
            : base(userId, storageOwnershipId)
        {
            InnerContainerId = innerContainerId;
        }
    }
}
