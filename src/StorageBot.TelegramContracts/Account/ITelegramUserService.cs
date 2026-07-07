using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.TelegramContracts.Account
{
    public interface ITelegramUserService
    {
        void AddByChat(Chat chat);

        int ResolveApplicationUserIdByChat(Chat chat);

        TelegramUser ResolveTelegramUserByUserId(int userId);

        TelegramUser ResolveTelegramUserByOwnershipId(int storageOwnershipId);
    }
}
