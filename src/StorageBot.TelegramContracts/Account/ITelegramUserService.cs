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
        ITelegramUser GetUser(Chat chat);

        int ResolveApplicationUserIdByChat(Chat chat);

        ITelegramUser ResolveTelegramUserByUserId(int userId);

        ITelegramUser ResolveTelegramUserByOwnershipId(int storageOwnershipId);
    }
}
