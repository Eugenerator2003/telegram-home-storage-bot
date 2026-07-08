using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface IMenuMessageStore
    {
        Task<int> GetMenuMessageId(ITelegramUser user);

        Task SetMenuMessageId(ITelegramUser user, int messageId); 
    }
}
