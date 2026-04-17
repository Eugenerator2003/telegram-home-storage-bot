using StorageBot.Application.UseCases.Storing.Containers.GetContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetContainers
{
    [TestClass]
    public class GetContainersInputTests
    {
        [TestMethod]
        [DataRow(1, 2, 3, 4)]
        [DataRow(3, 8, 99, null)]
        [DataRow(39, 78, 0, 25)]
        public void InitializeTest(int userId, int storageOwnershipId, int page, int? outerContainerId)
        {
            var input = new GetContainersInput(userId, storageOwnershipId, page, outerContainerId);
            Assert.AreEqual(userId, input.UserId);
            Assert.AreEqual(storageOwnershipId, input.StorageOwnershipId);
            Assert.AreEqual(page, input.Page);
            Assert.AreEqual(outerContainerId, input.OuterContainerId);
        }
    }
}
