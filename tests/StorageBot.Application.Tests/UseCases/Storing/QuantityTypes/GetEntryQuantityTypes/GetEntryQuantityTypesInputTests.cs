using StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    [TestClass]
    public class GetEntryQuantityTypesInputTests
    {
        [TestMethod]
        [DataRow(23, 3123, 10)]
        [DataRow(599, 66, 300)]
        [DataRow(2, 3, 1)]
        public void InitializeTest(int userId, int storageOwnershipId, int page)
        {
            var input = new GetEntryQuantityTypesInput(userId, storageOwnershipId, page);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(page, input.Page);
        }
    }
}
