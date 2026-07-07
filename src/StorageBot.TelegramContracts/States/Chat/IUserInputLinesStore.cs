using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface IUserInputLinesStore
    {
        Task SetLines(long id, IEnumerable<string> lines);

        Task<string[]> GetLines(long userId);

        Task ClearLines(long id);
    }
}
