using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing.Attributes.Actions.Base
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnknownCallbackQueryActionAttribute : Attribute
    {
    }
}
