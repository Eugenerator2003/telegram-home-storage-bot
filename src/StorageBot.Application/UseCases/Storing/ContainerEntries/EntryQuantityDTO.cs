using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries
{
    public class EntryQuantityDTO
    {
        public int TypeId { get; }

        public string Unit { get; }

        public bool IsInteger { get; }

        public decimal Value { get; }

        public EntryQuantityDTO(int typeId, string unit, bool isInteger, decimal value)
        {
            TypeId = typeId;
            Unit = unit;
            IsInteger = isInteger;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EntryQuantityDTO dto)
            {
                return false;
            }
            return TypeId == dto.TypeId &&
                   Unit.Equals(dto.Unit) &&
                   IsInteger == dto.IsInteger &&
                   Value == dto.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeId, Unit, IsInteger, Value);
        }
    }
}
