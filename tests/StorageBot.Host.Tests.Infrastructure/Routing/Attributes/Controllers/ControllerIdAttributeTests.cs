using StorageBot.Host.Routing.Attributes.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing.Attributes.Controllers
{
    [TestClass]
    public class ControllerIdAttributeTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(44)]
        public void InitializeTest(int id)
        {
            var attribute = new ControllerIdAttribute(id);
            Assert.AreEqual(id, attribute.Id);
        }
    }
}
