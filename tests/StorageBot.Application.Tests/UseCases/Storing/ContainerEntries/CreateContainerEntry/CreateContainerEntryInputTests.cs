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
        [DataRow(1, 2, 3, 4, "name", 0.3)]
        [DataRow(8665, 3721, 2679, 999, "2", 310)]
        [DataRow(31, 2139, 39, 675, "", 1000)]
        public void InitializeTest(int userId,
                                    int storageOwnershipId,
                                    int containerId,
                                    int entryQuantityTypeId,
                                    string entryName,
                                    double quantityValue)
        {
            var decimalValue = (decimal)quantityValue;
            var input = new CreateContainerEntryInput(userId, storageOwnershipId, containerId, entryQuantityTypeId, entryName, decimalValue);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(entryQuantityTypeId, input.EntryQuantityTypeId);
            Assert.AreEqual(decimalValue, input.QantityValue);
        }
    }
}
