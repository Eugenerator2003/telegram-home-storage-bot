using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface IUserInputStateStore
    {
        Task SetState(ITelegramUser user, InputState state);

        Task<InputState?> GetState(ITelegramUser user);

        Task ClearState(ITelegramUser user);
    }
}
