using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry
{
    public class DeleteContainerEntryInput : ContainerInput
    {
        public int ContainerEntryId { get; }

        public DeleteContainerEntryInput(int userId, int storageOwnershipId, int containerId, int containerEntryId) 
            : base(userId, storageOwnershipId, containerId)
        {
            ContainerEntryId = containerEntryId;
        }
    }
}
