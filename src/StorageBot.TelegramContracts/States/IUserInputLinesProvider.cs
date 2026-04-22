using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.StateManagement
{
    public interface IUserInputLinesProvider
    {
        Task SetLines(long id, IEnumerable<string> lines);

        Task<string[]> GetLines(long id);

        Task ClearLines(long id);
    }
}
