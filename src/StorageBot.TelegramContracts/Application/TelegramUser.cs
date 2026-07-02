using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.Application
{
    public class TelegramUser
    {
        public string Firstname { get; }
        public string Lastname { get; }
        public string Username { get; }
        public long TelegramId { get; }

        public TelegramUser(long telegramId, string firstname, string lastname, string username)
        {
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            TelegramId = telegramId;
        }
    }
}
