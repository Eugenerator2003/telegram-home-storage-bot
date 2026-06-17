using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public interface IHandlerInfoResolver
    {
        HandlerInfo GetHandlerInfo(int controllerId, int actionId);
    }
}
