using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes
{
    public class EntryQuantityTypeDTO
    {
        public int QuantityTypeId { get; }

        public bool IsInteger { get; }

        public string UnitName { get; }

        public EntryQuantityTypeDTO(int quantityTypeId, bool isInteger, string unitName)
        {
            QuantityTypeId = quantityTypeId;
            IsInteger = isInteger;
            UnitName = unitName;
        }
    }
}
