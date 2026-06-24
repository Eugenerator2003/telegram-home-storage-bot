using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StorageBot.TelegramContracts.Context
{
    public class MessageBotContext : BotContext
    {
        public Message Message { get; }

        public MessageBotContext(Update update) 
            : base(update)
        {
            Message = update.Message!;
        }

        protected override Chat GetChatFromUpdate(Update update)
        {
            return update.Message!.Chat;
        }

        public override void VerifyUpdate(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                throw new ArgumentException($"Expected update type: {UpdateType.CallbackQuery}", nameof(update));
            }
        }
    }
}
