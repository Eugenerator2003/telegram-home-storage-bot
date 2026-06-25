using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing.Attributes.Actions.Required
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MenuActionAttribute : RequiredActionAttribute
    {
    }
}
