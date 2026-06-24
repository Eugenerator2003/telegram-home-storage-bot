using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public class NavigationInfo
    {
        public int ControllerId { get; }

        public int ActionId { get; }

        public NavigationInfo(int controllerId, int actionId)
        {
            ControllerId = controllerId;
            ActionId = actionId;
        }
    }
}
