using StorageBot.Host.Routing.Attributes.Actions.Required;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public interface IRequiredNavigation
    {
        NavigationInfo GetRequiredAction<TAttribute>() where TAttribute : RequiredActionAttribute;
    }
}
