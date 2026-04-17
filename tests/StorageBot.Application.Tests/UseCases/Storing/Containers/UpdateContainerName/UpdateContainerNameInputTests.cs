using StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.UpdateContainerName
{
    [TestClass]
    public class UpdateContainerNameInputTests
    {
        [TestMethod]
        [DataRow(1, 1, 1, "container")]
        [DataRow(6, 3, 9, "")]
        [DataRow(3381, 88, 374774, "d")]
        public void InitializeTest(int userId, int storageOwnershipId, int containerId, string containerName)
        {
            var input = new UpdateContainerNameInput(userId, storageOwnershipId, containerId, containerName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(containerName, input.ContainerName);
        }
    }
}
