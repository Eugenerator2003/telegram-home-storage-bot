using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public interface IUserInputStateStore
    {
        Task SetState(long id, InputState state);

        Task<InputState?> GetState(long userId);

        Task ClearState(long id);
    }
}
