using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States
{
    public interface IMenuMessageProvider
    {
        Task<long> GetMenuMessageId(long userId);

        Task SetMenuMessageId(long userId, long messageId); 
    }
}
