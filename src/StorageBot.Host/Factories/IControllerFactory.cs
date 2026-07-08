using StorageBot.Host.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Factories
{
    public interface IControllerFactory
    {
        public BaseBotController GetController(Type controllerType);

        public T GetController<T>() where T : BaseBotController;
    }
}
