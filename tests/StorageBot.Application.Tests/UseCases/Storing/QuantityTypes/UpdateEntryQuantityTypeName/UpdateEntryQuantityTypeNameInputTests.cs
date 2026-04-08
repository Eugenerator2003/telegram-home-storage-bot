using StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName
{
    [TestClass]
    public class UpdateEntryQuantityTypeNameInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, "unit")]
        [DataRow(5, 7, 1, "pivo")]
        [DataRow(1, 2, 9, "elbow")]
        public void InitializeTest(int userId, int storageOwnershipId, int quantityTypeId, string unitName)
        {
            var input = new UpdateEntryQuantityTypeNameInput(userId, storageOwnershipId, quantityTypeId, unitName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(quantityTypeId, input.QuantityTypeId);
            Assert.AreEqual(unitName, input.UnitName);
        }
    }
}
