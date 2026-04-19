using StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    [TestClass]
    public class GetContainerEntriesInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4)]
        [DataRow(5, 2, 83, 986)]
        public void InitializeTest(int userId, int storageOwnershipId, int containerId, int page)
        {
            var input = new GetContainerEntriesInput(userId, storageOwnershipId, containerId, page);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(page, input.Page);
        }
    }
}
