using StorageBot.Application.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity
{
    [TestClass]
    public class UpdateContainerEntryQuantityInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4, 5.2)]
        [DataRow(100, 30, 4150, 123, 305.0)]
        [DataRow(132, 364, 931, 3, 2)]
        public void InitializeTest(int userId, int storageOwnershipId, int containerId, int containerEntryId, double quantityValueD)
        {
            var quantityValue = (decimal)quantityValueD;
            var input = new UpdateContainerEntryQuantityInput(userId, storageOwnershipId, containerId, containerEntryId, quantityValue);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(containerId, input.ContainerId);
            Assert.AreEqual(containerEntryId, input.ContainerEntryId);
            Assert.AreEqual(quantityValue, input.QuantityValue);
        }
    }
}
