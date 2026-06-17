using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.Chat
{
    public class InputState
    {
        public int ControllerId { get; }

        public int MethodId { get; }

        public InputState(int controllerId, int methodId)
        {
            ControllerId = controllerId;
            MethodId = methodId;
        }
    }
}
