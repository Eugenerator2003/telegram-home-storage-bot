using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess
{
    public class RelinquishStorageAccessInput : UserInput
    {
        public int StorageOwnerId { get; }

        public RelinquishStorageAccessInput(int userId, int storageOwnerId) : base(userId)
        {
            StorageOwnerId = storageOwnerId;
        }
    }
}
