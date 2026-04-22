using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.StateManagement
{
    public interface IUserInputStateProvider
    {
        Task SetState(long id, InputState state);

        Task<InputState?> GetState(long id);

        Task ClearState(long id);
    }
}
