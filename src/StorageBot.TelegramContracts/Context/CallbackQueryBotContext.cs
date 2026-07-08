using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StorageBot.TelegramContracts.Context
{
    public class CallbackQueryBotContext : BotContext
    {
        public string QueryId { get; }

        public string Data { get; }

        public CallbackQueryBotContext(Update update) 
            : base(update)
        {
            Data = update.CallbackQuery!.Data ?? string.Empty;
            QueryId = update.CallbackQuery.Id;
        }

        protected override void VerifyUpdate(Update update)
        {
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new ArgumentException($"Expected update type: {UpdateType.CallbackQuery}", nameof(update));
            }
        }

        protected override Chat GetChatFromUpdate(Update update)
        {
            return update.CallbackQuery!.Message!.Chat;
        }
    }
}
