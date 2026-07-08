using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface IUserInputLinesStore
    {
        Task SetLines(ITelegramUser user, IEnumerable<string> lines);

        Task<string[]> GetLines(ITelegramUser user);

        Task ClearLines(ITelegramUser user);
    }
}
