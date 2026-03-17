using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType
{
    public class CreateEntryQuantityTypeInput : StorageInput
    {
        public bool IsInteger { get; }

        public string UnitName { get; }

        public CreateEntryQuantityTypeInput(int userId, int storageOwnershipId, bool isInteger, string unitName) 
            : base(userId, storageOwnershipId)
        {
            IsInteger = isInteger;
            UnitName = unitName;
        }
    }
}
