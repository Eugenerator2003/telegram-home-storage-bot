using StorageBot.Host.Routing.Attributes.Actions.Updates;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing.Attributes.Actions.Updates
{
    [TestClass]
    public class ActionIdAttributeTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(3123)]
        public void InitializeTest(int id)
        {
            var attribute = new ActionIdAttribute(id);
            Assert.AreEqual(id, attribute.Id);
        }
    }
}
