using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName
{
    public class UpdateEntryQuantityTypeNameInput : StorageInput
    {
        public int QuantityTypeId { get; set; }

        public string UnitName { get; set; }

        public UpdateEntryQuantityTypeNameInput(int userId, int storageOwnershipId, int quantityTypeId, string unitName) 
            : base(userId, storageOwnershipId)
        {
            QuantityTypeId = quantityTypeId;
            UnitName = unitName;
        }
    }
}
