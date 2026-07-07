using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using StorageBot.Host.Controllers;
using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
using StorageBot.Host.Tests.Infrastructure.TestControllers;
using StorageBot.TelegramContracts.Account;
using StorageBot.TelegramContracts.States.Chat;
using StorageBot.TelegramContracts.States.ControllerState;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace StorageBot.Host.Tests.Infrastructure.Factories
{
    [TestClass]
    public class ControllerFactoryTests
    {
        private IServiceProvider serviceProvider = null!;

        [TestInitialize]
        public void Inititialize()
        {
            var services = new ServiceCollection();
            services.AddTransient<TestController1>();
            services.AddTransient<ControllerDependencies>();
            services.AddTransient((pr) => new Mock<ITelegramBotClient>().Object);
            services.AddTransient((pr) => new Mock<IStringLocalizer>().Object);
            services.AddTransient((pr) => new Mock<INavigation>().Object);
            services.AddTransient((pr) => new Mock<IRequiredNavigation>().Object);
            services.AddTransient((pr) => new Mock<IMenuMessageStore>().Object);
            services.AddTransient((pr) => new Mock<IRedirectBuffer>().Object);
            services.AddTransient((pr) => new Mock<IUserInputStateStore>().Object);
            services.AddTransient((pr) => new Mock<IUserInputLinesStore>().Object);
            services.AddTransient((pr) => new Mock<ITelegramUserService>().Object);
            services.AddTransient((pr) => new Mock<ICurrentOwnershipStore>().Object);
            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void GetControllerByTypeTest()
        {
            var factory = new ControllerFactory(serviceProvider);
            var controller = factory.GetController(typeof(TestController1));
            Assert.IsNotNull(controller);
            Assert.IsInstanceOfType<TestController1>(controller);
        }

        [TestMethod]
        public void GetControllerGenericTest()
        {
            var factory = new ControllerFactory(serviceProvider);
            var controller = factory.GetController<TestController1>();
            Assert.IsNotNull(controller);
            Assert.IsInstanceOfType<TestController1>(controller);
        }

        [TestMethod]
        public void GetControllerByTypeNotSubclassTest()
        {
            var factory = new ControllerFactory(serviceProvider);
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                factory.GetController(typeof(ControllerFactory));
            });
        }

        [TestMethod]
        public void GetControllerByTypeNotFoundTest()
        {
            var factory = new ControllerFactory(serviceProvider);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                factory.GetController(typeof(TestController2));
            });
        }

        [TestMethod]
        public void GetControllerByTypeDependenciesNotFoundTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<TestController1>()
                .BuildServiceProvider();
            var factory = new ControllerFactory(serviceProvider);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                factory.GetController(typeof(TestController1));
            });
        }
    }
}
