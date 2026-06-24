using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public interface IRequiredNavigation
    {
        NavigationInfo GetMenuAction();

        NavigationInfo GetUnregisteredCommandAction();

        NavigationInfo GetUnknownCallbackQueryAction();

        NavigationInfo GetUnexpectedMessageAction();
    }
}
