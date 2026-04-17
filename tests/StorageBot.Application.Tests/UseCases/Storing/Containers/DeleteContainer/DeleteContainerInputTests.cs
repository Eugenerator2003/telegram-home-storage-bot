using StorageBot.Application.UseCases.Storing.Containers.DeleteContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.DeleteContainer
{
    [TestClass]
    public class DeleteContainerInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(90, 66, 33)]
        public void IntializeTest(int userId, int storageOwnershipId, int containerId)
        {
            var input = new DeleteContainerInput(userId, storageOwnershipId, containerId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
        }
    }
}
