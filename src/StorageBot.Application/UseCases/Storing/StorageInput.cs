using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing
{
    public class StorageInput : UserInput
    {
        public int StorageOwnershipId { get; }

        public StorageInput(int userId, int storageOwnershipId) : base(userId)
        {
        }
    }
}
