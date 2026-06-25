using StorageBot.Host.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing
{
    [TestClass]
    public class NavigationInfoTests
    {
        [TestMethod]
        [DataRow(1, 3)]
        [DataRow(0, 540)]
        public void InitializeTest(int controllerId, int actionId)
        {
            var navigationInfo = new NavigationInfo(controllerId, actionId);
            Assert.AreEqual(controllerId, navigationInfo.ControllerId);
            Assert.AreEqual(actionId, navigationInfo.ActionId);
        }
    }
}
