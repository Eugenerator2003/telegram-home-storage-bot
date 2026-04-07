using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    [TestClass]
    public class DeleteEntryQuantityTypeInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(7, 5, 2)]
        public void InitailizeTest(int userId, int storageOwnershipId, int quantityTypeId)
        {
            var input = new DeleteEntryQuantityTypeInput(userId, storageOwnershipId, quantityTypeId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(quantityTypeId, input.QuantityTypeId);
        }
    }
}
