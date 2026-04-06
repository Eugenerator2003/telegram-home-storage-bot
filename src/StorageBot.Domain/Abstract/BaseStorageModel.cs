using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("StorageBot.Domain.Tests")]
[assembly: InternalsVisibleTo("StorageBot.Application.Tests")]

namespace StorageBot.Domain.Abstract
{
    public class BaseStorageModel
    {
        public int Id { get; internal set; }

        protected BaseStorageModel()
        {

        }

        protected BaseStorageModel(int id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            bool referenceEquals = ReferenceEquals(this, obj);

            if (referenceEquals)
            {
                return true;
            }

            if (Id == 0)
            {
                return referenceEquals;
            }
            else
            {
                return Id == ((BaseStorageModel)obj).Id;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
