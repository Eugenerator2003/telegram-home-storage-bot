using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface ICurrentOwnershipStore
    {
        Task<int> GetCurrentOwnership(long id);

        Task SetCurrentOwnership(long id, int ownershipId);
    }
}
