using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    public class CreateContainerEntryInput : ContainerInput
    {
        public int EntryQuantityTypeId { get; }
        public string EntryName { get; }

        public decimal QantityValue { get; }

        public CreateContainerEntryInput(int userId, 
                                         int storageOwnershipId, 
                                         int containerId, 
                                         int entryQuantityTypeId,
                                         string entryName,
                                         decimal quantityValue) 
            : base(userId, storageOwnershipId, containerId)
        {
            EntryQuantityTypeId = entryQuantityTypeId;
            EntryName = entryName;
            QantityValue = quantityValue;
        }
    }
}
