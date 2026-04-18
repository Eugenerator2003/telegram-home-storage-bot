using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    public class GetContainerEntriesInput : ContainerInput
    {
        public int Page { get; }

        public GetContainerEntriesInput(int userId, int storageOwnershipId, int containerId, int page) 
            : base(userId, storageOwnershipId, containerId)
        {
            Page = page;
        }
    }
}
