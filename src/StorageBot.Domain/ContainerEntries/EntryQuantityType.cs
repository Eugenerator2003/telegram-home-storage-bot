using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.ContainerEntries
{
    public class EntryQuantityType : BaseStorageModel
    {
        public string UnitName { get; set; }
        public bool IsInteger { get; set; }

        public EntryQuantityType(bool isInteger, string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
            {
                throw new ArgumentNullException("Unit name cannot be empty or null");
            }
            IsInteger = isInteger;

            UnitName = unitName;
        }

        protected EntryQuantityType()
        {
            UnitName = string.Empty;
        }

        public override string? ToString()
        {
            return $"Quantity type with {UnitName} units";
        }
    }
}
