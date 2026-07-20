using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    public class GetSingleEntryQuantityTypeInput : StorageInput
    {
        public int QuantityTypeId { get; }

        public GetSingleEntryQuantityTypeInput(int userId, int storageOwnershipId, int quantityTypeId) 
            : base(userId, storageOwnershipId)
        {
            QuantityTypeId = quantityTypeId;
        }
    }
}
