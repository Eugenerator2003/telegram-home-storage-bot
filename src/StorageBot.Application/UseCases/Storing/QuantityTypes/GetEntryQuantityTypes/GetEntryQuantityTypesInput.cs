using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    public class GetEntryQuantityTypesInput : StorageInput
    {
        public GetEntryQuantityTypesInput(int userId, int storageOwnershipId) 
            : base(userId, storageOwnershipId)
        {
        }
    }
}
