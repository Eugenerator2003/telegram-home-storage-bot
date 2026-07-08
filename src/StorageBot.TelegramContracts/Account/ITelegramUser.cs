using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.Account
{
    public interface ITelegramUser
    {
        public string Firstname { get; }
        public string Lastname { get; }
        public string Username { get; }
        public long TelegramId { get; }
    }
}
