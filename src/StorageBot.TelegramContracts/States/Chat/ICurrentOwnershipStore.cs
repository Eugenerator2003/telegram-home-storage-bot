using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface ICurrentOwnershipStore
    {
        Task<int> GetCurrentOwnership(ITelegramUser user);

        Task SetCurrentOwnership(ITelegramUser user, int ownershipId);
    }
}
