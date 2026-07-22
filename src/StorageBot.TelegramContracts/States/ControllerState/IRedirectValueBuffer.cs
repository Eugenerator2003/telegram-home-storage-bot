using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.TelegramContracts.States.ControllerState
{
    public interface IRedirectValueBuffer
    {
        void SetValue<T>(T value);

        T Consume<T>();
    }
}
