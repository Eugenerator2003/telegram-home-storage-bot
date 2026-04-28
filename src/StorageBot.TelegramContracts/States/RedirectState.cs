using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.TelegramContracts.States
{
    public class RedirectState
    {
        public int ControllerId { get; }

        public int ActionId { get; }

        public RedirectState(int controllerId, int actionId)
        {
            ControllerId = controllerId;
            ActionId = actionId;
        }
    }
}
