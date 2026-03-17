using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    public class DeleteEntryQuantityTypeInput : StorageInput
    {
        public int QuantityTypeId { get; }

        public DeleteEntryQuantityTypeInput(int userId, int storageOwnershipId, int quantitTypeId) 
            : base(userId, storageOwnershipId)
        {
            QuantityTypeId = quantitTypeId;
        }
    }
}
