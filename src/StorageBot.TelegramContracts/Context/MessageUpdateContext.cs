using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StorageBot.TelegramContracts.Context
{
    public class MessageUpdateContext : UpdateContext
    {
        public Message Message { get; }

        public MessageUpdateContext(Update update) 
            : base(update)
        {
            Message = update.Message!;
        }

        protected override Chat GetChatFromUpdate(Update update)
        {
            return update.Message!.Chat;
        }

        protected override void VerifyUpdate(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                throw new ArgumentException($"Expected update type: {UpdateType.CallbackQuery}", nameof(update));
            }
        }
    }
}
