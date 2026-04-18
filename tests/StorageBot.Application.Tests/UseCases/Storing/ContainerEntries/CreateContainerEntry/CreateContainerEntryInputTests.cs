using StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    [TestClass]
    public class CreateContainerEntryInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4, "name")]
        [DataRow(8665, 3721, 2679, 999, "2")]
        [DataRow(31, 2139, 39, 675, "")]
        public void InitializeTest(int userId,
                                    int storageOwnershipId,
                                    int containerId,
                                    int entryQuantityTypeId,
                                    string entryName)
        {
            var input = new CreateContainerEntryInput(userId, storageOwnershipId, containerId, entryQuantityTypeId, entryName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(entryQuantityTypeId, input.EntryQuantityTypeId);
        }
    }
}
