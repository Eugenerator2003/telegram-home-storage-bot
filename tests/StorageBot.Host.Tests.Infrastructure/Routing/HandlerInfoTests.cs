using StorageBot.Host.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing
{
    [TestClass]
    public class HandlerInfoTests
    {
        private class TestController
        {
            public void Hello()
            {

            }
        }

        [TestMethod]
        public void InitializeTest()
        {
            var type = typeof(TestController);
            var invoker = MethodInvoker.Create(type.GetMethod(nameof(TestController.Hello))!);
            var handlerInfo = new HandlerInfo(type, invoker);
            Assert.AreEqual(type, handlerInfo.ControllerType);
            Assert.AreEqual(invoker, handlerInfo.ActionInvoker);
        }

        [TestMethod]
        public void InitializeNullTypeTest()
        {
            Type type = null!;
            var invoker = MethodInvoker.Create(typeof(TestController).GetMethod(nameof(TestController.Hello))!);
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var handlerInfo = new HandlerInfo(type, invoker);
            });
        }

        [TestMethod]
        public void InitializeNullActionInvokerTest()
        {
            Type type = typeof(TestController);
            MethodInvoker invoker = null!;
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var handlerInfo = new HandlerInfo(type, invoker);
            });
        }
    }
}
