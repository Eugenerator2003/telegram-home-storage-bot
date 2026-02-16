using StorageBot.Domain.Abstract;
using StorageBot.Domain.Exceptions.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public class EntryQuantity
    {
        public decimal Value { get; private set; }
        public EntryQuantityType QuantityType { get; protected set; }

        private int _entryQuantityTypeId;

        public EntryQuantity(EntryQuantityType quantityType)
        {
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));

            _entryQuantityTypeId = quantityType.Id;
        }

        public EntryQuantity(EntryQuantityType quantityType, decimal value)
            : this(quantityType)
        {
            ValidateDecimalValue(value);
            Value = value;
        }

        public EntryQuantity(EntryQuantityType quantityType, string stringValue)
            : this(quantityType)
        {
            if (!decimal.TryParse(stringValue, out var decimalValue))
            {
                throw new InvalidQuantityValueException();
            }

            ValidateDecimalValue(decimalValue);
            Value = decimalValue;
        }

        protected EntryQuantity()
        {
            QuantityType = null!;
        }

        public EntryQuantity Add(EntryQuantity other)
        {
            ArgumentNullException.ThrowIfNull(other);
            if (!QuantityType.Equals(other.QuantityType))
            {
                throw new DifferentQuantityTypeException();
            }

            return new EntryQuantity(QuantityType, Value + other.Value);
        }

        public EntryQuantity Subtract(EntryQuantity other)
        {
            ArgumentNullException.ThrowIfNull(other);
            if (!QuantityType.Equals(other.QuantityType))
            {
                throw new DifferentQuantityTypeException();
            }
            decimal value = Value - other.Value;
            if (value <= 0)
            {
                throw new NotPositiveQuantityValueException();
            }
            return new EntryQuantity(QuantityType, value);
        }

        public override bool Equals(object? obj)
        {
            return obj is EntryQuantity quantity &&
                   Value == quantity.Value &&
                   QuantityType.Equals(quantity.QuantityType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, _entryQuantityTypeId);
        }

        public override string? ToString()
        {
            return $"Quantity {(QuantityType.IsInteger ? (int)Math.Truncate(Value) : Value)} {QuantityType.UnitName}";
        }

        protected void ValidateDecimalValue(decimal value)
        {
            if (value <= 0)
            {
                throw new NotPositiveQuantityValueException();
            }

            if (QuantityType.IsInteger && value % 1 != 0)
            {
                throw new NotIntegerValueException();
            }
        }
    }
}
