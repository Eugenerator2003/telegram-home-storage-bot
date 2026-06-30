using StorageBot.Host.Controllers;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Tests.Infrastructure.TestAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.TestControllers
{
    [ControllerId(0)]
    public class TestController1 : BotController
    {
        [ActionId(0)]
        [TestRequiredAttribute1]
        public async Task Method1()
        {

        }

        [ActionId(1)]
        [TestRequiredAttribute2]
        [Command("method2", null)]
        public async Task Method2()
        {

        }
    }
}
