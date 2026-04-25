using Microsoft.Extensions.Localization;
using StorageBot.Host.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Controllers
{
    public class ControllerDependencies
    {
        public IStringLocalizer Localizer { get; }

        public INavigationIdProvider Navigation { get; }

        public ControllerDependencies(IStringLocalizer localizer, INavigationIdProvider navigation)
        {
            ArgumentNullException.ThrowIfNull(localizer);
            ArgumentNullException.ThrowIfNull(navigation);
            Localizer = localizer;
            Navigation = navigation;
        }
    }
}
