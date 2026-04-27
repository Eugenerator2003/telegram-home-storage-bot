using StorageBot.Host.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public class ControllerTypeActionPair
    {
        public Type ControllerType { get; }

        public MethodInvoker ActionInvoker { get; }

        public ControllerTypeActionPair(Type controllerType, MethodInvoker actionInvoker)
        {
            ArgumentNullException.ThrowIfNull(controllerType);
            ArgumentNullException.ThrowIfNull(actionInvoker);
            ControllerType = controllerType;
            ActionInvoker = actionInvoker;
        }
    }
}
