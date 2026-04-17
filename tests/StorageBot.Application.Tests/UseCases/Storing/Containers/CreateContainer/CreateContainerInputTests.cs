using StorageBot.Application.UseCases.Storing.Containers.CreateContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.CreateContainer
{
    [TestClass]
    public class CreateContainerInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, "name")]
        [DataRow(20, 199, 230, "eman")]
        [DataRow(3, 2, null, "")]
        [DataRow(2, 3, null, "daa")]
        public void InitializeTest(int userId, int storageOwnershipId, int? outerContainerId, string name)
        {
            var input = new CreateContainerInput(userId, storageOwnershipId, outerContainerId, name);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(outerContainerId, input.OuterContainerId);
            Assert.AreEqual(name, input.ContainerName);
        }
    }
}
