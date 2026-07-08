using Microsoft.Extensions.DependencyInjection;
using StorageBot.Host.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Factories
{
    public class ControllerFactory : IControllerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ControllerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public BaseBotController GetController(Type controllerType)
        {
            if (!controllerType.IsSubclassOf(typeof(BaseBotController)))
            {
                throw new ArgumentException("Type must be a subclass of BotController");
            }
            var service = serviceProvider.GetService(controllerType) 
                ?? throw new InvalidOperationException($"Cannot create controller {controllerType.Name}: service not found");
            var controller = (BaseBotController)service;
            var dependencies = serviceProvider.GetService<ControllerDependencies>()
                ?? throw new InvalidOperationException($"Cannot initialize controller {controllerType.Name}: dependencies not found");
            controller.SetDependencies(dependencies);
            return controller;
        }

        public T GetController<T>() where T : BaseBotController
        {
            return (T)GetController(typeof(T));
        }
    }
}
