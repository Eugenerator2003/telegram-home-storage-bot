using StorageBot.Host.Routing.Attributes.Actions.Updates;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing.Attributes.Actions.Updates
{
    [TestClass]
    public class CommandAttributeTests
    {
        [TestMethod]
        [DataRow("command", null)]
        [DataRow("say_hello", "command to say hello")]
        public void InitializeTest(string name, string? description)
        {
            var attribute = new CommandAttribute(name, description);
            Assert.AreEqual(name, attribute.Name);
            Assert.AreEqual(description, attribute.Description);
        }
    }
}
