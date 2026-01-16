using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public abstract class EntryQuantity
    {
        public EntryQuantityType QuantityType { get; protected set; }

        private int _entryQuantityTypeId;

        public EntryQuantity(EntryQuantityType quantityType)
        {
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));

            _entryQuantityTypeId = quantityType.Id;
        }

        public EntryQuantity(EntryQuantityType quantityType, string value)
            : this(quantityType)
        {
            SetValue(value);
        }

        protected EntryQuantity()
        {
            QuantityType = null!;
        }

        public EntryQuantity Add(EntryQuantity quantity)
        {
            Type thisType = GetType();
            Type anotherType = quantity.GetType();
            if (thisType != anotherType)
            {
                throw new InvalidOperationException($"Can not add quantity of another type. {thisType.Name} and {anotherType.Name} are not the same");
            }

            return AddQuantity(quantity);
        }

        protected abstract EntryQuantity AddQuantity(EntryQuantity quantity);

        protected abstract void SetValue(string value);
    }
}
