using StorageBot.TelegramContracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StorageBot.TelegramContracts.Context
{
    public abstract class UpdateContext
    {
        public Chat Chat { get; }

        public UpdateType UpdateType { get; }

        public Update Update { get; }

        public UpdateContext(Update update)
        {
            ArgumentNullException.ThrowIfNull(update);
            VerifyUpdate(update);
            Update = update;
            UpdateType = update.Type;
            Chat = GetChatFromUpdate(update);
        }

        protected abstract void VerifyUpdate(Update update);

        protected abstract Chat GetChatFromUpdate(Update update);
    }
}
