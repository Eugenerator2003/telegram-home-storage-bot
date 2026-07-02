using Microsoft.Extensions.Localization;
using Moq;
using StorageBot.Host.Controllers;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.Application;
using StorageBot.TelegramContracts.States.Chat;
using StorageBot.TelegramContracts.States.ControllerState;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telegram.Bot;

namespace StorageBot.Host.Tests.Infrastructure.Controllers
{
    [TestClass]
    public class ControllerDependenciesTests
    {
        private static object GetMock(Type type)
        {
            var mockType = typeof(Mock<>).MakeGenericType(type);
            return ((Mock)Activator.CreateInstance(mockType)!).Object;
        }

        [TestMethod]
        public void InitializeTest()
        {
            var botClient = new Mock<ITelegramBotClient>().Object;
            var localizer = new Mock<IStringLocalizer>().Object;
            var navigation = new Mock<INavigation>().Object;
            var requiredNavigation = new Mock<IRequiredNavigation>().Object;
            var menuStore = new Mock<IMenuMessageStore>().Object;
            var redirectBuffer = new Mock<IRedirectBuffer>().Object;
            var stateStore = new Mock<IUserInputStateStore>().Object;
            var linesStore = new Mock<IUserInputLinesStore>().Object;
            var telegramUserService = new Mock<ITelegramUserService>().Object;
            var ownershipStore = new Mock<ICurrentOwnershipStore>().Object;
            var controllerDependencies = new ControllerDependencies(
                botClient,
                localizer,
                navigation,
                requiredNavigation,
                menuStore,
                redirectBuffer,
                linesStore,
                stateStore,
                ownershipStore,
                telegramUserService
                );
            Assert.AreEqual(botClient, controllerDependencies.BotClient);
            Assert.AreEqual(localizer, controllerDependencies.Localizer);
            Assert.AreEqual(navigation, controllerDependencies.Navigation);
            Assert.AreEqual(requiredNavigation, controllerDependencies.RequiredNavigation);
            Assert.AreEqual(menuStore, controllerDependencies.MenuStore);
            Assert.AreEqual(redirectBuffer, controllerDependencies.RedirectBuffer);
            Assert.AreEqual(stateStore, controllerDependencies.StateStore);
            Assert.AreEqual(linesStore, controllerDependencies.LinesStore);
        }

        [TestMethod]
        public void InitializeNullTest()
        {
            var constructor = typeof(ControllerDependencies).GetConstructors().First();
            var objects = constructor.GetParameters().Select(p => GetMock(p.ParameterType)).ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                var constructorObjects = new object?[objects.Length];
                objects.CopyTo(constructorObjects, 0);
                constructorObjects[i] = null;
                var exception = Assert.Throws<Exception>(() =>
                {
                    constructor.Invoke(constructorObjects);
                });
                Assert.IsInstanceOfType<ArgumentNullException>(exception.InnerException);
            }
        }
    }
}
