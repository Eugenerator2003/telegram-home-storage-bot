using StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    [TestClass]
    public class GetSingleEntryQuantityTypeInputTests
    {
        [TestMethod]
        [DataRow(1, 30, 400)]
        [DataRow(10, 3, 2)]
        public void InitializeTest(int userId, int storageOwnershipId, int quantityTypeId)
        {
            var input = new GetSingleEntryQuantityTypeInput(userId, storageOwnershipId, quantityTypeId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(quantityTypeId, input.QuantityTypeId);
        }
    }
}
