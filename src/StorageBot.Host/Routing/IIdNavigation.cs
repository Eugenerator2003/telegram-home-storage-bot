using StorageBot.Host.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public interface IIdNavigation
    {
        int GetControllerId(string controllerName);

        int GetActionId<T>(string actionName) where T : BotController;

        int GetMainControllerId();

        int GetMenuActionId();
    }
}
