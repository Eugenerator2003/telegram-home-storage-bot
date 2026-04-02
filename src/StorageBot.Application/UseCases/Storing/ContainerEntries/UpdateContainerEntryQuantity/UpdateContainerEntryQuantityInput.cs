using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity
{
    public class UpdateContainerEntryQuantityInput 
        : ContainerInput
    {
        public int ContainerEntryId { get; }

        public decimal QuantityValue { get; }

        public UpdateContainerEntryQuantityInput(int userId, int storageOwnershipId, int containerId, int containerEntryId, decimal quantityValue) 
            : base(userId, storageOwnershipId, containerId)
        {
            ContainerEntryId = containerEntryId;
            QuantityValue = quantityValue;
        }
    }
}
