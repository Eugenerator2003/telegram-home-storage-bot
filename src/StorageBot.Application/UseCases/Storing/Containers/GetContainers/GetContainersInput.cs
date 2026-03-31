using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetContainers
{
    public class GetContainersInput : StorageInput
    {
        public int Page { get; }

        public int? OuterContainerId { get; } 
        public GetContainersInput(int userId, int storageOwnershipId, int page, int? outerContainerId) 
            : base(userId, storageOwnershipId)
        {
            Page = page;
            OuterContainerId = outerContainerId;
        }
    }
}
