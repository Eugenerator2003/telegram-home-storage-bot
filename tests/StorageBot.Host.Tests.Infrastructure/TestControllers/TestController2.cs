using StorageBot.Host.Controllers;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Tests.Infrastructure.TestAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.TestControllers
{
    [ControllerId(1)]
    public class TestController2 : BotController
    {
        [ActionId(0)]
        [TestRequiredAttribute3]
        public async Task Method1()
        {

        }
    }
}
