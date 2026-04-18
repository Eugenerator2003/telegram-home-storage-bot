using StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.DeleteContainerEntry
{
    [TestClass]
    public class DeleteContainerEntryInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4)]
        [DataRow(4, 3, 2, 1)]
        [DataRow(199, 299, 312893, 39)]
        public void InitializeTest(int userId, int storageOwnershipId, int containerId, int containerEntryId)
        {
            var input = new DeleteContainerEntryInput(userId, storageOwnershipId, containerId, containerEntryId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(containerEntryId, input.ContainerEntryId);
        }
    }
}
