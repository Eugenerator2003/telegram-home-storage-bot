using StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.CreateEntryQuantityType
{
    [TestClass]
    public class CreateEntryQuantityTypeInputTests
    {
        [TestMethod]
        [DataRow(1, 1, false, "1-unit")]
        [DataRow(1, 3, true, "kilo")]
        [DataRow(2, 130000, true, "gram")]
        public void InitializeTest(int userId, int storageOwnershipId, bool isInteger, string unitName)
        {
            var input = new CreateEntryQuantityTypeInput(userId, storageOwnershipId, isInteger, unitName);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(isInteger, input.IsInteger);
            Assert.AreEqual(unitName, input.UnitName);
        }
    }
}
