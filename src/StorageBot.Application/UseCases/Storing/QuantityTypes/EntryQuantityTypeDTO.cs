using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes
{
    public class EntryQuantityTypeDTO
    {
        public int Id { get; }

        public bool IsInteger { get; }

        public string UnitName { get; }

        public EntryQuantityTypeDTO(int quantityTypeId, bool isInteger, string unitName)
        {
            Id = quantityTypeId;
            IsInteger = isInteger;
            UnitName = unitName;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EntryQuantityTypeDTO dto) return false;
            return Id == dto.Id &&
                   IsInteger == dto.IsInteger &&
                   UnitName.Equals(dto.UnitName);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsInteger, UnitName);
        }
    }
}
