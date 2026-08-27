using StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetSingleContainer
{
    [TestClass]
    public class GetSingleContainerInputTests
    {
        [TestMethod]
        [DataRow(10, 20, 30)]
        [DataRow(33, 22, 11)]
        public void InitializeTest(int userId, int storageOwnershipId, int containerId)
        {
            var input = new GetSingleContainerInput(userId, storageOwnershipId, containerId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
        }
    }
}
